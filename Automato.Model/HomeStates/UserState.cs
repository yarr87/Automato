using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.HomeStates
{
    /// <summary>
    /// Represents the state of a single user
    /// </summary>
    public class UserState
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Presence flag
        /// </summary>
        public bool IsHome { get; set; }
    }
}
