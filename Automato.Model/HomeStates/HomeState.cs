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

        public HomeState Copy()
        {
            return new HomeState()
            {
                Time = this.Time,
                Lights = this.Lights.Select(l => l.Copy()).ToList(),
                Users = this.Users.Select(u => u.Copy()).ToList()
            };
        }

        // TODO:
        // outside weather
        // inside temperature
    }
}
