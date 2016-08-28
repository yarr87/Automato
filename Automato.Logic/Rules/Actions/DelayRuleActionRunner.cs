using Automato.Integration;
using Automato.Logic.Stores;
using Automato.Model.Rules;
using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Actions
{
    /// <summary>
    /// Execute a rule action that waits for a time and then resumes the rest of the actions
    /// </summary>
    public class DelayRuleActionRunner : BaseRuleActionRunner<DelayRuleAction>
    {
        public override async Task<IEnumerable<BaseRuleAction>> ExecuteActionAsync(DelayRuleAction action, IEnumerable<BaseRuleAction> nextActions)
        {
            var startTime = DateTime.Now.Add(action.Delay);

            // A listener in Automato.Job will react to this and start a timer
            // Passing startTime rather than just the timespan in case there's a delay in firing the listener
            var delayedAction = new DelayedAction() { Actions = nextActions, StartTime = startTime };
            new DelayedActionStore().Save(delayedAction);

            return await Task.FromResult(Enumerable.Empty<BaseRuleAction>());
        } 

        protected override Task ExecuteActionAsync(DelayRuleAction action)
        {
            throw new Exception("Cannot execute DelayRuleAction without other actions");
        }
    }
}
