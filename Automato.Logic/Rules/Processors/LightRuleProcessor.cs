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
        public override Task<bool> IsRuleActive(LightRule rule, HomeState state)
        {
            var light = state.Lights.FirstOrDefault(l => l.InternalName == rule.LightState.InternalName);

            bool isActive = light != null && NormalizeState(light.State) == NormalizeState(rule.LightState.State);

            return Task.FromResult(isActive);
        }

        private string NormalizeState(string state)
        {
            if (state == "0") return "OFF";
            else if (state == "100") return "ON";
            else return state;
        }
    }
}
