using Automato.Integration.Model;
using Automato.Integration.Router;
using Automato.Logic;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automato.Job
{
    /// <summary>
    /// Periodically checks for devices connected to the local network and notifies the api of changes.
    /// </summary>
    public class UserPresenceDetector
    {
        private TimeSpan timeSpan = TimeSpan.FromSeconds(30);
        private Timer _timer;
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Time that a device must be disconnected to consider the user no longer home.
        /// </summary>
        private TimeSpan TimeToConsiderActuallyGone = TimeSpan.FromMinutes(4);

        /// <summary>
        /// Local cache of devices, from the last time loaded
        /// </summary>
        private List<NetworkDevice> _networkDevices = null;

        /// <summary>
        /// Track when each device first disconnects.  A device must be disconnected for a certain amount of time before we send an
        /// update, since sometimes our phones like to disconnect for just a minute randomly.
        /// </summary>
        private Dictionary<string, DateTime> _timeFirstLeft = new Dictionary<string, DateTime>();

        public void Start()
        {
            Logger.Debug("Starting up user presence detector");
            _timer = new Timer(new TimerCallback(DoWork), null, TimeSpan.Zero, timeSpan);
        }

        private void DoWork(object state)
        {
            DoWorkAsync().Wait();
        }

        private async Task DoWorkAsync()
        {
            try
            {
                using (var router = new RouterApi())
                {
                    var devices = await new RouterApi().GetNetworkDevices();

                    await OnDevicesLoaded(devices);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing user presence", ex);
            }
        }

        private async Task OnDevicesLoaded(IEnumerable<NetworkDevice> devices)
        {
            var users = new UserStore().GetAll();

            var devicesWithUsers = devices.Where(d => users.Any(u => u.DeviceMac == d.Mac)).ToList();

            if (devicesWithUsers.Any())
            {
                // When a device first drops off, track the time so we can only mark a device
                // as disconnected if it's been long enough.
                foreach (var device in devicesWithUsers)
                {
                    if (!device.Status)
                    {
                        if (users.First(d => d.DeviceMac == device.Mac).IsHome && !_timeFirstLeft.ContainsKey(device.Mac))
                        {
                            Logger.DebugFormat("Device {0} dropped off first time", device.Mac);
                            _timeFirstLeft.Add(device.Mac, DateTime.Now);
                        }
                    }
                    else
                    {
                        if (_timeFirstLeft.ContainsKey(device.Mac))
                        {
                            Logger.DebugFormat("Device {0} reconnected", device.Mac);
                            _timeFirstLeft.Remove(device.Mac);
                        }
                    }
                }

                List<NetworkDevice> updates;

                // Not first time, compare with previous data to only get changes in user presence
                if (_networkDevices != null)
                {
                    var joined = devicesWithUsers
                                    .Join(_networkDevices, d => d.Mac, d => d.Mac, (newDevice, oldDevice) => new { newDevice, oldDevice });

                    // Newly connected devices - immediately send updates for devices that are connected
                    var home = joined
                                .Where(d => d.newDevice.Status && !d.oldDevice.Status)
                                .Where(d => users.Any(u => u.DeviceMac == d.newDevice.Mac && !u.IsHome))
                                .Select(d => d.newDevice);

                    // Disconnected devices - only those that have been off for long enough, to avoid the random 1-2 minute disconnects
                    // that our phones like to do.
                    var notHome = joined
                                    .Where(d => !d.newDevice.Status)
                                    .Where(d => users.Any(u => u.DeviceMac == d.newDevice.Mac && u.IsHome))
                                    .Where(d => _timeFirstLeft.ContainsKey(d.newDevice.Mac) && DateTime.Now - _timeFirstLeft[d.newDevice.Mac] > TimeToConsiderActuallyGone)
                                    .Select(d => d.newDevice)
                                    .ToList();

                    updates = home.Concat(notHome).ToList();

                    // Not that these devices are gone, take them out of the dictionary
                    foreach (var disconnectedDevice in notHome)
                    {
                        _timeFirstLeft.Remove(disconnectedDevice.Mac);
                    }
                                    
                }
                // First run, send all data
                else
                {
                    updates = devicesWithUsers;
                }

                await OnChangesDetected(updates, _networkDevices == null);
            }

            // Save for next time
            _networkDevices = devicesWithUsers;
        }

        /// <summary>
        /// Notify the api of any updates
        /// </summary>
        /// <param name="deviceUpdates"></param>
        /// <param name="isInitialDataLoad"></param>
        /// <returns></returns>
        private async Task OnChangesDetected(IEnumerable<NetworkDevice> deviceUpdates, bool isInitialDataLoad)
        {
            if (deviceUpdates.Any())
            {
                await new ApiClient().SendUserPresenceUpdates(deviceUpdates, isInitialDataLoad);
            }
        }
    }
}
