using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    /// <summary>
    /// Types of actions that can be taken when a rule triggers
    /// </summary>
    public enum RuleActionType
    {
        /// <summary>
        /// Turn a light on or off
        /// </summary>
        Light,

        /// <summary>
        /// Send an email
        /// </summary>
        Email,

        /// <summary>
        /// Send an email that will be delivered as a text message
        /// </summary>
        EmailAsText,

        /// <summary>
        /// Change the thermostat temperature
        /// </summary>
        Temperature,

        /// <summary>
        /// Send a sonos command
        /// </summary>
        Sonos

        // Other ideas
        // flicker a light on and off
        // change thermostat
        // play music
        // speak
    }
}
