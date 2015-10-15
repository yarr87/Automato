using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// Represent's a device's state.  Used to send messages from Job to Api when device states are updated, and
    /// when getting current state from openhab.
    /// </summary>
    public class DeviceState
    {
        public string InternalName { get; set; }
        public string State { get; set; }
    }
}
