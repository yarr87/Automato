using Automato.Model.HomeStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    // Man this took forever to get right

    public interface IRuleProcessor
    {
        bool IsRuleActive(IRuleDefinition rule, HomeState state);
    }

    public interface IRuleProcessor<T> where T : IRuleDefinition
    {
        bool IsRuleActive(T rule, HomeState state); 
    }
}
