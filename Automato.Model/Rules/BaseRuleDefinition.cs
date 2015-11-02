using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    public abstract class BaseRuleDefinition : IRuleDefinition
    {
        public abstract RuleType RuleType { get; }

        /// <summary>
        /// Flag to tell if this rule is triggered.  When true, responds to an event (ie, a light turning on, someone coming home).  When
        /// false, just responds to the current state.
        /// </summary>
        public bool IsTriggered { get; set; }
    }
}
