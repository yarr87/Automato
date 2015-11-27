﻿using Automato.Integration;
using Automato.Logic.Rules.Actions;
using Automato.Model.Rules;
using Automato.Model.Rules.Actions;
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
            { RuleActionType.EmailAsText, new EmailAsTextRuleActionRunner() }
        };

        public async Task RunRule(Rule rule)
        {
            foreach (var action in rule.Actions)
            {
                var runner = _actionRunners[action.ActionType];

                if (runner != null)
                {
                    await runner.ExecuteActionAsync(action);
                }
            }
        }
    }
}
