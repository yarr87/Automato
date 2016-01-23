using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// A rule is a combination of rule definitions and a corresponding actions to take if they are active.
    /// This object is saved to the database.
    /// </summary>
    public class Rule : IActionable, ICopyable<Rule>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDisabled { get; set; }

        public IEnumerable<BaseRuleDefinition> RuleDefinitions { get; set; }

        public IEnumerable<BaseRuleAction> Actions { get; set; }
        
        public int ExecutionCount { get; set; }

        public DateTime? LastExecuted { get; set; }

        public void CopyTo(Rule destination)
        {
            destination.Name = this.Name;
            destination.Description = this.Description;
            destination.IsDisabled = this.IsDisabled;
            destination.Actions = this.Actions;
            destination.RuleDefinitions = this.RuleDefinitions;
        }
    }
}
