using Automato.Data;
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
            using (var db = new TomatoContext())
            {
                var query = db.Devices.Include("Tags");
                var sql = query.ToString();

                return query.ToList();
                return db.Devices.Include("Tags").ToList();
            }

            //var xml = XDocument.Load(_xmlPath);

            //DeviceList devices = new DeviceList();

            //XmlSerializer ser = new XmlSerializer(typeof(DeviceList));
            
            //using (var reader = xml.CreateReader())
            //{
            //    devices = (DeviceList)ser.Deserialize(reader);
            //}

            //return devices.Devices;
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
            using (var db = new TomatoContext())
            {
                return db.Devices.FirstOrDefault(d => d.Id == id);
            }

            //var components = GetDevices();

            //return components.FirstOrDefault(c => c.Id == id);
        }

        public void AddOrEditDevice(Device device)
        {
            using (var db = new TomatoContext())
            {
                if (device.Id == 0)
                {
                    db.Devices.Add(device);
                }
                else
                {
                    var existing = db.Devices.FirstOrDefault(d => d.Id == device.Id);

                    if (existing != null)
                    {
                        existing.CopyFrom(device);
                    }
                }


                db.SaveChanges();
            }
        }

        public void DeleteById(int id)
        {
            using (var db = new TomatoContext())
            {
                var device = db.Devices.FirstOrDefault(d => d.Id == id);

                if (device != null)
                {
                    db.Devices.Remove(device);

                    db.SaveChanges();
                }
            }
        }
    }
}
