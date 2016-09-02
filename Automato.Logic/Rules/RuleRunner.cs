using Automato.Integration;
using Automato.Logic.Rules.Actions;
using Automato.Model.Rules;
using Automato.Model.Rules.Actions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules
{
    /// <summary>
    /// Executes rules
    /// </summary>
    public class RuleRunner
    {
        private Dictionary<RuleActionType, IRuleActionRunner> _actionRunners = new Dictionary<RuleActionType, IRuleActionRunner>()
        {
            { RuleActionType.Light, new LightRuleActionRunner() },
            { RuleActionType.EmailAsText, new EmailAsTextRuleActionRunner() },
            { RuleActionType.Temperature, new TemperatureRuleActionRunner() },
            { RuleActionType.Sonos, new SonosRuleActionRunner() },
            { RuleActionType.Delay, new DelayRuleActionRunner() }
        };

        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task RunRule(IActionable actionable)
        {
            Logger.DebugFormat("Running rule with actions {0}", actionable.Actions);
            var actions = actionable.Actions;

            while (actions.Any())
            {
                var action = actions.First();
                var rest = actions.Skip(1);

                var runner = _actionRunners[action.ActionType];

                if (runner != null)
                {
                    actions = await runner.ExecuteActionAsync(action, rest);
                }
                else
                {
                    actions = rest;
                }
            }

            //foreach (var action in actionable.Actions)
            //{
            //    var runner = _actionRunners[action.ActionType];

            //    if (runner != null)
            //    {
            //        await runner.ExecuteActionAsync(action);
            //    }
            //}
        }
    }
}
