using Automato.Model.HomeStates;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Processors
{
    public abstract class BaseRuleProcessor<T> : IRuleProcessor<T>, IRuleProcessor where T : class, IRuleDefinition
    {
        public abstract bool IsRuleActive(T rule, HomeState state);

        bool IRuleProcessor.IsRuleActive(IRuleDefinition rule, HomeState state)
        {
            return IsRuleActive(rule as T, state);
        }
    }
}
