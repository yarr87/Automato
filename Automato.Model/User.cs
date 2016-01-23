using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// Represents a user whose presence can be known
    /// </summary>
    public class User : ICopyable<User>
    {
        /// <summary>
        /// Auto-gen id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mac address of the user's device, which can be used to detect presence based on its 
        /// connection to the network
        /// </summary>
        public string DeviceMac { get; set; }

        /// <summary>
        /// True if the user is home, false otherwise.  We guess this based on their device's connection,
        /// or could possibly override manually.
        /// </summary>
        public bool IsHome { get; set; }

        /// <summary>
        /// Email address of this user we can send to
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// An email address that we can use to send text messages to this user
        /// </summary>
        public string TextAddress { get; set; }

        public void CopyTo(User destination)
        {
            destination.Name = this.Name;
            destination.DeviceMac = this.DeviceMac;
            destination.Email = this.Email;
            destination.TextAddress = this.TextAddress;
        }
    }
}
