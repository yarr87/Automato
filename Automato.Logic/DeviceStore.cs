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
                                                select new
                                                {
                                                    tag = tag,
                                                    map = map
                                                }
                               select new
                               {
                                   device = device,
                                   tags = deviceTags.ToList()
                               });
                var devices = device1.ToList();

                var allDevices = devices.Select(d =>
                {
                    var device = d.device;
                    device.Tags = new List<Tag>();

                    // Make sure the isIndirect property is included on each tag from the map table
                    foreach (var tagWrapper in d.tags)
                    {
                        // Some of the tagWrapper.tag objects are shared, so we can't update them directly
                        Tag tag = new Tag() { Id = tagWrapper.tag.Id };
                        tag.CopyFrom(tagWrapper.tag);
                        tag.IsIndirect = tagWrapper.map.IsIndirect;
                        
                        device.Tags.Add(tag);
                    }

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
            RemoveParentTags(device.Tags);

            using (var db = new TomatoContext())
            {
                var allTags = db.Tags.ToList();

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

                    var parentTags = GetParentTags(tags, allTags);

                    foreach (var parentTag in parentTags)
                    {
                        parentTag.IsIndirect = true;
                        db.DeviceTagMaps.Add(new DeviceTagMap()
                        {
                            TagId = parentTag.Id,
                            DeviceId = device.Id,
                            IsIndirect = true
                        });
                        tags.Add(parentTag);
                    }

                    db.SaveChanges();

                    device.Tags = tags;
                }
                else
                {
                    var existing = db.Devices.FirstOrDefault(d => d.Id == device.Id);

                    if (existing != null)
                    {
                        var maps = db.DeviceTagMaps.Where(m => m.DeviceId == existing.Id).ToList();

                        // Remove maps that were deleted.  This will also remove indirect tags, which will be added later.
                        for(var i = maps.Count - 1; i >= 0; i--)
                        {
                            var map = maps[i];

                            if (!device.Tags.Any(t => t.Id == map.TagId) || map.IsIndirect.GetValueOrDefault())
                            {
                                db.DeviceTagMaps.Remove(map);
                                maps.RemoveAt(i);
                            }
                        }

                        // Save the deletes first
                        db.SaveChanges();

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

                        var parentTags = GetParentTags(device.Tags, allTags);

                        foreach (var parentTag in parentTags)
                        {
                            parentTag.IsIndirect = true;
                            db.DeviceTagMaps.Add(new DeviceTagMap()
                            {
                                TagId = parentTag.Id,
                                DeviceId = device.Id,
                                IsIndirect = true
                            });
                        }

                        // This needs to be after all the map updates or bad things happen
                        existing.CopyFrom(device);

                        // Can't save with tags
                        var tags = device.Tags;
                        existing.Tags = null;
                        
                        db.SaveChanges();

                        tags.AddRange(parentTags);

                        existing.Tags = tags;
                        device.Tags = tags;
                    }
                }
            }
        }

        /// <summary>
        /// Return a list of all parent tags for the given list.  These will be indirect tags on a device.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private List<Tag> GetParentTags(List<Tag> tags, List<Tag> allTags)
        {
            var parentTags = new List<Tag>();

            foreach (var tag in tags)
            {
                parentTags.AddRange(GetParentTags(tag, allTags));
            }

            return parentTags;
        }

        private List<Tag> GetParentTags(Tag tag, List<Tag> allTags)
        {
            var parentTags = new List<Tag>();

            if (tag.ParentId.HasValue)
            {
                var parent = allTags.FirstOrDefault(t => t.Id == tag.ParentId);

                if (parentTags.Any(t => t.Id == parent.Id))
                {
                    throw new InvalidOperationException("Tag cycle detected");
                }

                parentTags.Add(parent);

                parentTags.AddRange(GetParentTags(parent, allTags));
            }

            return parentTags;
        }

        /// <summary>
        /// Give a list of tags, remove the ones that are unnecessary because they are parents
        /// of other tags
        /// </summary>
        private void RemoveParentTags(List<Tag> tags)
        {
            var toDelete = new List<Tag>();

            foreach (var tag in tags)
            {
                if (tag.ParentId != null)
                {
                    var parent = tags.FirstOrDefault(t => t.Id == tag.ParentId);

                    if (parent != null)
                    {
                        toDelete.Add(parent);
                    }
                }
            }

            foreach (var del in toDelete)
            {
                tags.Remove(del);
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
