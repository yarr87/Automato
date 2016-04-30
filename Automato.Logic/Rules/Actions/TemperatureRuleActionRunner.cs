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
            await new TemperatureHandler().SetTemperature(action.ThermostatId, action.Temperature);
        }
    }
}
