using Automato.Model.Rules.Actions;
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
        public abstract Task ExecuteActionAsync(T action);

        async Task IRuleActionRunner.ExecuteActionAsync(IRuleAction action)
        {
            await ExecuteActionAsync(action as T);
        }
    }
}
