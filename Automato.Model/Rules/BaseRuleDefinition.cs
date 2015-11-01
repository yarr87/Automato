using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    public abstract class BaseRuleDefinition : IRuleDefinition
    {
        public abstract RuleType RuleType { get; }
    }
}
