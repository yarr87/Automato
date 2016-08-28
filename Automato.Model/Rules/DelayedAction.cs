using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// When executing a DelayAction, all actions after it in the rule are saved as a DelayedAction object.  The job listens for documents
    /// of this type and starts a timer when it receives one.  This way we can schedule delays that run outside the web api context.
    /// </summary>
    public class DelayedAction : ICopyable<DelayedAction>, IActionable
    {
        public string Id { get; set; }

        public DateTime StartTime { get; set; }

        public IEnumerable<BaseRuleAction> Actions { get; set; }

        public void CopyTo(DelayedAction destination)
        {
            throw new NotImplementedException();
        }
    }
}
