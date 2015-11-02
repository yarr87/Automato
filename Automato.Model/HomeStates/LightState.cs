using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.HomeStates
{
    /// <summary>
    /// The state of a single light
    /// </summary>
    public class LightState
    {
        /// <summary>
        /// Internal name of the light
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// State of the light
        /// </summary>
        public string State { get; set; }

        public LightState Copy()
        {
            return new LightState()
            {
                InternalName = this.InternalName,
                State = this.State
            };
        }
    }
}
