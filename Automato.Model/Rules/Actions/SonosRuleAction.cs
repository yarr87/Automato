using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    public class SonosRuleAction : BaseRuleAction
    {
        public override RuleActionType ActionType
        {
            get { return RuleActionType.Sonos; }
        }

        /// <summary>
        /// Name of the sonos device this applies to
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of command to send
        /// </summary>
        public SonosActionType CommandType { get; set; }

        /// <summary>
        /// Parameter to send with the command
        /// </summary>
        public string Parameter { get; set; }
    }
}
