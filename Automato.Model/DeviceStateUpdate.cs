using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// Used to send messages from Job to Api when device states are updated
    /// </summary>
    public class DeviceStateUpdate
    {
        public string InternalName { get; set; }
        public string State { get; set; }
    }
}
