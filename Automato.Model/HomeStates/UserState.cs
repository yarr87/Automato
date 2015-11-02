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
        /// Id of the user
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Presence flag
        /// </summary>
        public bool IsHome { get; set; }

        public UserState Copy()
        {
            return new UserState()
            {
                UserId = this.UserId,
                IsHome = this.IsHome
            };
        }
    }
}
