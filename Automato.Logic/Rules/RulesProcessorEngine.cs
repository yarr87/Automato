using Automato.Logic.Rules.Processors;
using Automato.Model.HomeStates;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules
{
    /// <summary>
    /// Figures out which rules are active
    /// </summary>
    public class RulesProcessorEngine
    {
        private Dictionary<RuleType, IRuleProcessor> _ruleProcessors = new Dictionary<RuleType, IRuleProcessor>()
        {
            { RuleType.Light, new LightRuleProcessor() },
            { RuleType.Time, new TimeRuleProcessor() },
            { RuleType.User, new UserRuleProcessor() },
            { RuleType.Day, new DayRuleProcessor() }
        };

        /// <summary>
        /// Return the rules in the given list that were inactive in the previous state and are active in the current state
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Rule>> GetNewlyActiveRules(List<Rule> rules, HomeState previousState, HomeState currentState)
        {
            var activeRules = new List<Rule>();

            foreach (var rule in rules)
            {
                if (await DidRuleChangeToActive(rule, previousState, currentState))
                {
                    activeRules.Add(rule);
                }
            }

            return activeRules;

            //return await rules.Where(async r => await DidRuleChangeToActive(r, previousState, currentState));
        }

        /// <summary>
        /// Return true if the given rule is active for the given state
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public async Task<bool> IsRuleActive(Rule rule, HomeState currentState)
        {
            // Not sure how to do this with await and linq, so just looping
            var isCurrentActive = true;

            foreach (var ruleDef in rule.RuleDefinitions)
            {
                isCurrentActive &= await IsRuleDefinitionActive(ruleDef, currentState);
            }

            return isCurrentActive;
        }

        /// <summary>
        /// Return true if the given rule switched from inactive to active between the two given states
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public async Task<bool> DidRuleChangeToActive(Rule rule, HomeState previousState, HomeState currentState)
        {
            return await IsRuleActive(rule, currentState) && !await IsRuleActive(rule, previousState);
            // Check if it's fully active with the current state and wasn't on the previous state
            //return rule.RuleDefinitions.All(d => IsRuleDefinitionActive(d, currentState).Result) && !rule.RuleDefinitions.All(async d => await IsRuleDefinitionActive(d, previousState));
        }

        private async Task<bool> IsRuleDefinitionActive(BaseRuleDefinition ruleDefinition, HomeState state)
        {
            return await _ruleProcessors[ruleDefinition.RuleType].IsRuleActive(ruleDefinition, state);
        }
    }
}
