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
        public IEnumerable<Device> GetDevices()
        {
            using (var db = new TomatoContext())
            {
                var device1 = (from device in db.Devices
                               let deviceTags = from map in db.DeviceTagMaps
                                                join tag in db.Tags on map.TagId equals tag.Id
                                                where map.DeviceId == device.Id
                                                select tag
                               select new
                               {
                                   device = device,
                                   tags = deviceTags.ToList()
                               });
                var devices = device1.ToList();

                var allDevices = devices.Select(d =>
                {
                    var device = d.device;
                    device.Tags = d.tags;
                    return device;
                }).ToList();

                return allDevices;
            }
        }

        public Device GetDeviceById(int id)
        {
            using (var db = new TomatoContext())
            {
                return db.Devices.FirstOrDefault(d => d.Id == id);
            }
        }

        public void AddOrEditDevice(Device device)
        {
            using (var db = new TomatoContext())
            {
                // New device
                if (device.Id == 0)
                {
                    // Need to save device without tags because it's not configured right to save them automatically
                    var tags = device.Tags;
                    device.Tags = null;

                    db.Devices.Add(device);
                    db.SaveChanges();

                    // Save these after the device so we know its id
                    foreach (var tag in tags)
                    {
                        var map = new DeviceTagMap()
                        {
                            DeviceId = device.Id,
                            TagId = tag.Id
                        };

                        db.DeviceTagMaps.Add(map);
                    }

                    device.Tags = tags;

                    db.SaveChanges();
                }
                else
                {
                    var existing = db.Devices.FirstOrDefault(d => d.Id == device.Id);

                    if (existing != null)
                    {
                        var maps = db.DeviceTagMaps.Where(m => m.DeviceId == existing.Id).ToList();

                        // Remove maps that were deleted
                        foreach (var map in maps)
                        {
                            if (!device.Tags.Any(t => t.Id == map.TagId))
                            {
                                db.DeviceTagMaps.Remove(map);
                            }
                        }

                        // Add new maps
                        foreach (var tag in device.Tags)
                        {
                            if (!maps.Any(m => m.TagId == tag.Id))
                            {
                                db.DeviceTagMaps.Add(new DeviceTagMap()
                                {
                                    TagId = tag.Id,
                                    DeviceId = device.Id
                                });
                            }
                        }

                        existing.CopyFrom(device);

                        // Can't save with tags
                        var tags = device.Tags;
                        existing.Tags = null;

                        db.SaveChanges();

                        existing.Tags = tags;
                    }
                }
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

                    // Delete maps for this device
                    var maps = db.DeviceTagMaps.Where(m => m.DeviceId == id).ToList();

                    foreach (var map in maps)
                    {
                        db.DeviceTagMaps.Remove(map);
                    }

                    db.SaveChanges();
                }
            }
        }
    }
}
