using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    /// <summary>
    /// An action that changes the thermostat's temperature
    /// </summary>
    public class TemperatureRuleAction : BaseRuleAction
    {
        public override RuleActionType ActionType
        {
            get { return RuleActionType.Temperature; }
        }

        public string ThermostatId { get; set; }

        public string Temperature { get; set; }
    }
}
