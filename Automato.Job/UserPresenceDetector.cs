using Automato.Integration.Model;
using Automato.Integration.Router;
using Automato.Logic;
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

        /// <summary>
        /// Local cache of devices, from the last time loaded
        /// </summary>
        private List<NetworkDevice> _networkDevices = null;

        public void Start()
        {
            _timer = new Timer(new TimerCallback(DoWork), null, TimeSpan.Zero, timeSpan);
        }

        private void DoWork(object state)
        {
            DoWorkAsync().Wait();
        }

        private async Task DoWorkAsync()
        {        
            using (var router = new RouterApi())
            {
                var devices = await new RouterApi().GetNetworkDevices();

                await OnDevicesLoaded(devices);
            }
        }

        private async Task OnDevicesLoaded(IEnumerable<NetworkDevice> devices)
        {
            var users = new UserStore().GetUsers();

            var devicesWithUsers = devices.Where(d => users.Any(u => u.DeviceMac == d.Mac)).ToList();

            if (devicesWithUsers.Any())
            {
                List<NetworkDevice> updates;

                // Not first time, compare with previous data to only get changes in user presence
                if (_networkDevices != null)
                {
                    updates = devicesWithUsers
                                .Join(_networkDevices, d => d.Mac, d => d.Mac, (newDevice, oldDevice) => new { newDevice, oldDevice })
                                .Where(d => d.newDevice.Status != d.oldDevice.Status)
                                .Select(d => d.newDevice)
                                .ToList();
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
