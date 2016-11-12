using Automato.Model.HomeStates;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Processors
{
    // Man this took forever to get right

    public interface IRuleProcessor
    {
        Task<bool> IsRuleActive(IRuleDefinition rule, HomeState state);
    }

    public interface IRuleProcessor<T> where T : IRuleDefinition
    {
        Task<bool> IsRuleActive(T rule, HomeState state);
    }
}
