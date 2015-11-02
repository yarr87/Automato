using Automato.Integration;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules
{
    /// <summary>
    /// Executes rules
    /// </summary>
    public class RuleRunner
    {
        public async Task RunRule(Rule rule)
        {
            foreach (var deviceState in rule.Action.DeviceStates)
            {
                await new OpenHabRestService().SendCommand(deviceState.InternalName, deviceState.State);
            }
        }
    }
}
