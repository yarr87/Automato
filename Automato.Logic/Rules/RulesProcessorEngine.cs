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
        public IEnumerable<Rule> GetNewlyActiveRules(List<Rule> rules, HomeState previousState, HomeState currentState)
        {
            return rules.Where(r => DidRuleChangeToActive(r, previousState, currentState));
        }

        /// <summary>
        /// Return true if the given rule is active for the given state
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public bool IsRuleActive(Rule rule, HomeState currentState)
        {
            return rule.RuleDefinitions.All(d => IsRuleDefinitionActive(d, currentState));
        }

        /// <summary>
        /// Return true if the given rule switched from inactive to active between the two given states
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public bool DidRuleChangeToActive(Rule rule, HomeState previousState, HomeState currentState)
        {
            // Check if it's fully active with the current state and wasn't on the previous state
            return rule.RuleDefinitions.All(d => IsRuleDefinitionActive(d, currentState)) && !rule.RuleDefinitions.All(d => IsRuleDefinitionActive(d, previousState));
        }

        private bool IsRuleDefinitionActive(BaseRuleDefinition ruleDefinition, HomeState state)
        {
            return _ruleProcessors[ruleDefinition.RuleType].IsRuleActive(ruleDefinition, state);
        }
    }
}
