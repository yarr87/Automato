using Automato.Model.Rules.Actions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Actions
{
    /// <summary>
    /// Base class for a rule action runner, which executes rule actions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRuleActionRunner<T> : IRuleActionRunner<T>, IRuleActionRunner where T : class, IRuleAction
    {
        protected abstract Task ExecuteActionAsync(T action);
        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        async Task<IEnumerable<BaseRuleAction>> IRuleActionRunner.ExecuteActionAsync(IRuleAction action, IEnumerable<BaseRuleAction> nextActions)
        {
            return await ExecuteActionAsync(action as T, nextActions);
        }

        public virtual async Task<IEnumerable<BaseRuleAction>> ExecuteActionAsync(T action, IEnumerable<BaseRuleAction> nextActions)
        {
            await ExecuteActionAsync(action);

            return nextActions;
        }
    }
}
