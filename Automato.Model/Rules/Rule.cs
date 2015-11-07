using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// A rule is a combination of rule definitions and a corresponding action to take if they are active.
    /// This object is saved to the database.
    /// </summary>
    public class Rule
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<BaseRuleDefinition> RuleDefinitions { get; set; }

        public RuleAction Action { get; set; }

        public int ExecutionCount { get; set; }

        public DateTime? LastExecuted { get; set; }
    }
}
