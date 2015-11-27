using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    /// <summary>
    /// A rule action that will text a user by sending to a specific email address
    /// </summary>
    public class EmailAsTextRuleAction : BaseRuleAction
    {
        public override RuleActionType ActionType
        {
            get { return RuleActionType.EmailAsText; }
        }

        /// <summary>
        /// Id of the user to send a text to
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The message to send
        /// </summary>
        public string Message { get; set; }
    }
}
