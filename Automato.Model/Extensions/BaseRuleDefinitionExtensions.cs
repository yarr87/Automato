using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Extensions
{
    public static class BaseRuleDefinitionExtensions
    {
        public static IEnumerable<LightRule> GetLightRules(this IEnumerable<BaseRuleDefinition> ruleDefs)
        {
            return ruleDefs.Where(r => r is LightRule).Select(r => r as LightRule);
        }

        public static IEnumerable<UserRule> GetUserRules(this IEnumerable<BaseRuleDefinition> ruleDefs)
        {
            return ruleDefs.Where(r => r is UserRule).Select(r => r as UserRule);
        }
    }
}
