using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// Device that triggers a scene.
    /// Most likely this is a single minimote button, triggered via an openhab rule when the button is pressed
    /// that then runs the scene via /api/scenes/trigger/{triggerInternalName}
    /// </summary>
    public class SceneTrigger : ICopyable<SceneTrigger>
    {
        public string Id { get; set; }

        // TODO: some sort of type param?  What other types are there besides minimote?  Just generic one-off trigger I guess?  Would be nice to have
        // any arbitrary url to trigger from another system if desired.

        /// <summary>
        /// Used internally to group triggers.  For example, all buttons on a minimote have the same parent.  Usually
        /// this will just be a friendly name for the remote
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Id within the given ParentId.  Used for minimotes to indicate which button this trigger applies to.
        /// </summary>
        public string SubId { get; set; }

        /// <summary>
        /// Friendly name of the trigger
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Internal OpenHab name of the trigger
        /// </summary>
        public string TriggerInternalName { get; set; }

        /// <summary>
        /// Id of the scene that gets triggered
        /// </summary>
        public string SceneId { get; set; }

        public void CopyTo(SceneTrigger destination)
        {
            destination.Name = this.Name;
            destination.TriggerInternalName = this.TriggerInternalName;
            destination.SceneId = this.SceneId;
        }
    }
}
