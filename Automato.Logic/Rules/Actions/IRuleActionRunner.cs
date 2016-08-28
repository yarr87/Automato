using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Actions
{
    public interface IRuleActionRunner
    {
        /// <summary>
        /// Execute an action, and return the rest of the actions to run next.  Allows an action to defer execution of other
        /// actions to later (for delay actions).
        /// </summary>
        Task<IEnumerable<BaseRuleAction>> ExecuteActionAsync(IRuleAction action, IEnumerable<BaseRuleAction> nextActions);
    }

    public interface IRuleActionRunner<T> where T : IRuleAction
    {
        Task<IEnumerable<BaseRuleAction>> ExecuteActionAsync(T action, IEnumerable<BaseRuleAction> nextActions);
    }
}
