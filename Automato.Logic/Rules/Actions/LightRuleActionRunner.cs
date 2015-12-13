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
    /// Execute a rule action that turns a light on or off
    /// </summary>
    public class LightRuleActionRunner : BaseRuleActionRunner<LightRuleAction>
    {
        public override async Task ExecuteActionAsync(LightRuleAction action)
        {
            await new OpenHabRestService().SendCommand(action.DeviceState.InternalName, action.DeviceState.State);
        }
    }
}
