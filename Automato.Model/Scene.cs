using Automato.Model.Rules;
using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// A scene is a group of actions that are taken by a single trigger.  That trigger will be from an api url, hit from the scenes
    /// page or could be configured from a remote button.
    /// </summary>
    public class Scene : IActionable, ICopyable<Scene>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<BaseRuleAction> Actions { get; set; }

        public void CopyTo(Scene destination)
        {
            destination.Name = this.Name;
            destination.Description = this.Description;
            destination.Actions = this.Actions;
        }
    }
}
