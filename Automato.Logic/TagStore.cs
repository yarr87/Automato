using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic
{
    public class TagStore
    {
        public IEnumerable<Tag> GetTags()
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                
                var tags = session.Query<Tag>().ToList();

                // Ordering in memory because can't do this order query in raven
                // TODO: could probably do this with an index
                return tags.OrderBy(t => string.IsNullOrWhiteSpace(t.ParentId) ? 0 : 1).ThenBy(t => t.Name);
            }

            //using (var db = new TomatoContext())
            //{
            //    var query = db.Tags.OrderBy(t => t.ParentId.HasValue).ThenBy(t => t.Name);
                    
            //    var data = query.ToList();

            //    return data;
            //}
        }

        public void AddOrEditTag(Tag tag)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                // Client sends empty string for new devices (null breaks stuff), but we need to send null to the db
                // to generate the correct id
                if (tag.Id == string.Empty)
                {
                    tag.Id = null;
                }
                else
                {
                    var devicesWithTag = session.Query<Device>().Where(d => d.Tags.Any(t => t.Id == tag.Id)).ToList();

                    foreach (var device in devicesWithTag)
                    {
                        var deviceTag = device.Tags.FirstOrDefault(t => t.Id == tag.Id);

                        if (deviceTag != null)
                        {
                            deviceTag.Name = tag.Name;
                            deviceTag.ParentId = tag.ParentId;
                            deviceTag.IsDirect = false;

                            session.Store(device);
                        }
                    }
                }

                session.Store(tag);

                session.SaveChanges();
            }

            //using (var db = new TomatoContext())
            //{
                //if (tag.Id == 0)
                //{
                //    db.Tags.Add(tag);
                //}
                //else
                //{
                //    var existing = db.Tags.FirstOrDefault(d => d.Id == tag.Id);

                //    if (existing != null)
                //    {
                //        existing.CopyFrom(tag);
                //    }
                //}

                //db.SaveChanges();
            //}
        }

        public void DeleteById(string id)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                var devicesWithTag = session.Query<Device>().Where(d => d.Tags.Any(t => t.Id == id)).ToList();

                foreach (var device in devicesWithTag)
                {
                    device.Tags = device.Tags.Where(t => t.Id != id).ToList();
                    session.Store(device);
                }

                session.Delete(id);

                session.SaveChanges();

                // TODO: delete all references in devices
            }

            //using (var db = new TomatoContext())
            //{
                //var tag = db.Tags.FirstOrDefault(d => d.Id == id);

                //if (tag != null)
                //{
                //    db.Tags.Remove(tag);

                //    // Delete maps for this tag
                //    var maps = db.DeviceTagMaps.Where(m => m.TagId == id).ToList();

                //    foreach (var map in maps)
                //    {
                //        db.DeviceTagMaps.Remove(map);
                //    }

                //    db.SaveChanges();
                //}
            //}
        }
    }
}
