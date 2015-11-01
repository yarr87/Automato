using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// Actions taken when a rule is triggered
    /// </summary>
    public class RuleAction
    {
        public IEnumerable<DeviceState> DeviceStates { get; set; }
    }
}
