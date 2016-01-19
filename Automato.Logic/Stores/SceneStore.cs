using Automato.Logic.Stores;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic
{
    public class SceneStore : BaseStore<Scene>
    {
        protected override Func<Scene, string> SortExpr
        {
            get
            {
                return (r) => r.Name;
            }
        }

        public Scene GetById(string id)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                return session.Load<Scene>(id);
            }
        }

        public void Save(Scene scene)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                // Client sends empty string for new devices (null breaks stuff), but we need to send null to the db
                // to generate the correct id
                if (string.IsNullOrWhiteSpace(scene.Id))
                {
                    scene.Id = null;

                    session.Store(scene);
                }
                else
                {
                    // Don't overwrite existing stats on save
                    var existingScene = session.Load<Scene>(scene.Id);

                    if (existingScene != null)
                    {
                        existingScene.Name = scene.Name;
                        existingScene.Description = scene.Description;
                        existingScene.Actions = scene.Actions;
                    }
                    else
                    {
                        throw new Exception(string.Format("Saving scene with id {0} but that id wasn't found in the database", scene.Id));
                    }
                }

                session.SaveChanges();
            }
        }
    }
}
