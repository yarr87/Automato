using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    /// <summary>
    /// An action that turns a light on or off
    /// </summary>
    public class LightRuleAction : BaseRuleAction
    {
        public override RuleActionType ActionType
        {
            get { return RuleActionType.Light; }
        }

        public DeviceState DeviceState { get; set; }

        public override string ToString()
        {
            return string.Format("LightRuleAction with DeviceState = {0} {1}", DeviceState.InternalName, DeviceState.State);
        }
    }
}
