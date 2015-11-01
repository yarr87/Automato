using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.HomeStates
{
    /// <summary>
    /// Represents the entire state of the house at a point in time
    /// </summary>
    public class HomeState
    {
        public DateTime Time { get; set; }

        public IEnumerable<LightState> Lights { get; set; }

        public IEnumerable<UserState> Users { get; set; }

        // TODO:
        // outside weather
        // inside temperature
    }
}
