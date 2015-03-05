using Automato.Data;
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
            using (var db = new TomatoContext())
            {
                return db.Tags.ToList();
            }
        }

        public void AddOrEditTag(Tag tag)
        {
            using (var db = new TomatoContext())
            {
                if (tag.Id == 0)
                {
                    db.Tags.Add(tag);
                }
                else
                {
                    var existing = db.Tags.FirstOrDefault(d => d.Id == tag.Id);

                    if (existing != null)
                    {
                        existing.CopyFrom(tag);
                    }
                }

                db.SaveChanges();
            }
        }

        public void DeleteById(int id)
        {
            using (var db = new TomatoContext())
            {
                var tag = db.Tags.FirstOrDefault(d => d.Id == id);

                if (tag != null)
                {
                    db.Tags.Remove(tag);

                    // Delete maps for this tag
                    var maps = db.DeviceTagMaps.Where(m => m.TagId == id).ToList();

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
