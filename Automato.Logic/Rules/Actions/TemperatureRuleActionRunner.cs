using Automato.Integration;
using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Actions
{
    /// <summary>
    /// Execute a rule action that changes the thermostat's temperature
    /// </summary>
    public class TemperatureRuleActionRunner : BaseRuleActionRunner<TemperatureRuleAction>
    {
        public override async Task ExecuteActionAsync(TemperatureRuleAction action)
        {
            // This is basically the same as the lights version, but could be updated in the future.  Just trying to get
            // it working quickly now.
            await new OpenHabRestService().SendCommand(action.DeviceState.InternalName, action.DeviceState.State);
        }
    }
}
