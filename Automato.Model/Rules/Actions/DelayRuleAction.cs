using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    /// <summary>
    /// An action that delays actions that come after it
    /// </summary>
    public class DelayRuleAction : BaseRuleAction
    {
        public override RuleActionType ActionType
        {
            get { return RuleActionType.Delay; }
        }

        public TimeSpan Delay { get; set; }
    }
}
