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
    public class RulesEngine
    {
        private Dictionary<RuleType, IRuleProcessor> _ruleProcessors = new Dictionary<RuleType, IRuleProcessor>()
        {
            { RuleType.Light, new LightRuleProcessor() },
            { RuleType.Time, new TimeRuleProcessor() },
            { RuleType.User, new UserRuleProcessor() }
        };

        public IEnumerable<Rule> GetActiveRules(List<Rule> rules, HomeState previousState, HomeState currentState)
        {
            return rules.Where(r => IsRuleActive(r, previousState, currentState));
        }

        private bool IsRuleActive(Rule rule, HomeState previousState, HomeState currentState)
        {
            // TODO: group rules into and/or
            return rule.RuleDefinitions.All(d => IsRuleDefinitionActive(d, previousState, currentState));
        }
        
        private bool IsRuleDefinitionActive(BaseRuleDefinition ruleDefinition, HomeState previousState, HomeState currentState)
        {
            var processor = _ruleProcessors[ruleDefinition.RuleType];

            var isActive = processor.IsRuleActive(ruleDefinition, currentState);

            // A rule definition is active if it's active on the current state and wasn't active on the previou state
            return isActive && !processor.IsRuleActive(ruleDefinition, previousState);
        }
    }
}
