using Automato.Logic.Stores;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic
{
    public class SceneTriggerStore : BaseStore<SceneTrigger>
    {
        protected override Func<SceneTrigger, string> SortExpr
        {
            get
            {
                return (r) => r.Name;
            }
        }

        public SceneTrigger GetByInternalName(string triggerInternalName)
        {
            var triggers = GetAll();

            return triggers.FirstOrDefault(t => t.TriggerInternalName == triggerInternalName);
        }

        public void Save(IEnumerable<SceneTrigger> sceneTriggers)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                var existing = session.Query<SceneTrigger>().ToList();

                foreach (var sceneTrigger in sceneTriggers)
                {
                    // Client sends empty string for new devices (null breaks stuff), but we need to send null to the db
                    // to generate the correct id
                    if (string.IsNullOrWhiteSpace(sceneTrigger.Id))
                    {
                        sceneTrigger.Id = null;

                        session.Store(sceneTrigger);
                    }
                    else
                    {
                        // Don't overwrite existing stats on save
                        var existingScene = existing.FirstOrDefault(e => e.Id == sceneTrigger.Id);

                        if (existingScene != null)
                        {
                            sceneTrigger.CopyTo(existingScene);
                        }
                        else
                        {
                            throw new Exception(string.Format("Saving scene trigger with id {0} but that id wasn't found in the database", sceneTrigger.Id));
                        }
                    }
                }

                var deleted = existing.Where(e => !sceneTriggers.Any(s => s.Id == e.Id));

                foreach (var d in deleted)
                {
                    session.Delete(d.Id);
                }

                session.SaveChanges();
            }
        }
    }
}
