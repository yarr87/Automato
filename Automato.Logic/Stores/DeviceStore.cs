using Automato.Integration;
using Automato.Logic.Stores;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Automato.Logic
{
    public class DeviceStore : BaseStore<Device>
    {
        protected override Func<Device, string> SortExpr
        {
            get
            {
                return (d) => d.Name;
            }
        }

        public async Task<IEnumerable<Device>> GetDevices()
        {
            var devices = GetDevicesWithoutState();

            // Add current state info
            var states = await new OpenHabRestService().GetDeviceStates();

            foreach (var device in devices)
            {
                var state = states.FirstOrDefault(s => s.InternalName == device.InternalName);

                if (state != null)
                {
                    device.State = state.State;
                }
            }

            return devices;
        }

        private IEnumerable<Device> GetDevicesWithoutState()
        {
            return GetAll();
        }

        public Device GetDeviceById(int id)
        {
            return null;// db.Devices.FirstOrDefault(d => d.Id == id);
        }

        public void AddOrEditDevice(Device device)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                // Client sends empty string for new devices (null breaks stuff), but we need to send null to the db
                // to generate the correct id
                if (device.Id == string.Empty)
                {
                    device.Id = null;
                }

                var allTags = session.Query<Tag>().ToList();
                var parentTags = new List<DeviceTag>();

                foreach (var deviceTag in device.Tags)
                {
                    parentTags.AddRange(GetParentTags(deviceTag, allTags));
                }

                device.Tags.AddRange(parentTags);

                // Remove dups (if someone adds a tag and its parent)
                device.Tags = device.Tags.Distinct(new DeviceTagComparer()).ToList();

                foreach (var deviceTag in device.Tags)
                {
                    // A tag is directly on the device if there are no other tags pointing to it via parent id.
                    // ie, for Kitchen Lights, Kitchen = Direct, Downstairs = InDirect (via Kitchen.ParentId)
                    deviceTag.IsDirect = !device.Tags.Any(t => t.ParentId == deviceTag.Id);
                }

                session.Store(device);

                session.SaveChanges();
            }
        }

        public void ProcessStateUpdates(IEnumerable<DeviceState> states)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                var devices = session.Query<Device>().ToList();

                foreach (var state in states)
                {
                    var device = devices.FirstOrDefault(d => d.InternalName == state.InternalName);

                    if (device != null)
                    {
                        device.LastStateChange = DateTime.Now;
                    }
                }

                session.SaveChanges();
            }
        }

        private List<DeviceTag> GetParentTags(DeviceTag deviceTag, List<Tag> allTags)
        {
            var parentTags = new List<DeviceTag>();

            if (!string.IsNullOrWhiteSpace(deviceTag.ParentId))
            {
                var parent = allTags.FirstOrDefault(t => t.Id == deviceTag.ParentId);

                if (parentTags.Any(t => t.Id == parent.Id))
                {
                    throw new InvalidOperationException("Tag cycle detected");
                }

                var deviceTagParent = new DeviceTag
                {
                    Id = parent.Id,
                    ParentId = parent.ParentId,
                    Name = parent.Name,
                    IsDirect = false
                };

                parentTags.Add(deviceTagParent);

                parentTags.AddRange(GetParentTags(deviceTagParent, allTags));
            }

            return parentTags;
        }
    }
}
