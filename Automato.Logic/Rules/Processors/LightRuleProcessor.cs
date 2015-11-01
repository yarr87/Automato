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
    /// Checks if a light rule is active
    /// </summary>
    public class LightRuleProcessor : BaseRuleProcessor<LightRule>
    {
        public override bool IsRuleActive(LightRule rule, HomeState state)
        {
            var light = state.Lights.FirstOrDefault(l => l.InternalName == rule.LightState.InternalName);

            return light != null && light.State == rule.LightState.State;
        }
    }
}
