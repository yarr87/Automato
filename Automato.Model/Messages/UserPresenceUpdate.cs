using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Messages
{
    /// <summary>
    /// A message used to update a user's presence
    /// </summary>
    public class UserPresenceUpdate
    {
        /// <summary>
        /// MAC address of the user's device, which identifies them
        /// </summary>
        public string DeviceMac { get; set; }

        /// <summary>
        /// Presence flag
        /// </summary>
        public bool IsHome { get; set; }

        /// <summary>
        /// Flag to indicate that this update is just the system initializing, and updates shouldn't
        /// be treated as triggers
        /// </summary>
        public bool IsInitializationOnly { get; set; }
    }
}
