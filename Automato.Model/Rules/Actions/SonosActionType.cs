using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    public enum SonosActionType
    {
        /// <summary>
        /// Play a sonos favorite
        /// </summary>
        Favorite,

        /// <summary>
        /// Play a sonos playlist
        /// </summary>
        Playlist,

        /// <summary>
        /// Pause current track
        /// </summary>
        Pause
    }
}
