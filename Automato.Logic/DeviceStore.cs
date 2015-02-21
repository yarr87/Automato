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
    public class DeviceStore
    {
        private const string _xmlPath = @"c:\users\jeff\documents\visual studio 2013\Projects\Automato\Automato.Web\App_Data\devices.xml";

        public IEnumerable<Device> GetDevices()
        {
            var xml = XDocument.Load(_xmlPath);

            DeviceList devices = new DeviceList();

            XmlSerializer ser = new XmlSerializer(typeof(DeviceList));
            
            using (var reader = xml.CreateReader())
            {
                devices = (DeviceList)ser.Deserialize(reader);
            }

            return devices.Devices;
            //ser.Deserialize(xml.CreateReader());

            //return new List<Component>()
            //{
            //    new Component() { Name = "Light 1", State = "OFF", Type = ComponentType.LightSwitch },
            //    new Component() { Name = "Light 2", State = "OFF", Type = ComponentType.LightSwitch },
            //    new Component() { Name = "Dimmer 1", State = "40", Type = ComponentType.Dimmer }
            //};
        }

        public Device GetDeviceById(int id)
        {
            var components = GetDevices();

            return components.FirstOrDefault(c => c.Id == id);
        }

        public void AddOrEditDevice(Device device)
        {
            var devices = GetDevices() ?? new List<Device>();

            var deviceList = new DeviceList();
            deviceList.Devices = devices.ToList();

            // Add
            if (device.Id == 0)
            {
                device.Id = (devices.Any() ? devices.Max(c => c.Id) + 1 : 1);

                deviceList.Devices.Add(device);
            }
            // Edit
            else
            {
                var existing = devices.FirstOrDefault(c => c.Id == device.Id);

                if (existing != null)
                {
                    existing.CopyFrom(device);
                }
            }

            XmlSerializer ser = new XmlSerializer(typeof(DeviceList));

            using (var stream = File.OpenWrite(_xmlPath)) 
            {
                stream.SetLength(0);
                ser.Serialize(stream, deviceList);
            }
        }
    }
}
