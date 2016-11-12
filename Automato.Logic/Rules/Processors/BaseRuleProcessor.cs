using Automato.Model.HomeStates;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Processors
{
    /// <summary>
    /// Base class for a rule processor, which can detect if a specific rule definition type is active
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRuleProcessor<T> : IRuleProcessor<T>, IRuleProcessor where T : class, IRuleDefinition
    {
        public abstract Task<bool> IsRuleActive(T rule, HomeState state);

        async Task<bool> IRuleProcessor.IsRuleActive(IRuleDefinition rule, HomeState state)
        {
            return await IsRuleActive(rule as T, state);
        }
    }
}
