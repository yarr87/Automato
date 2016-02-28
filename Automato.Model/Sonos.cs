using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model
{
    /// <summary>
    /// A single Sonos player
    /// </summary>
    public class Sonos : ICopyable<Sonos>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public IEnumerable<string> Favorites { get; set; }

        [NotMapped]
        public IEnumerable<string> Playlists { get; set; }

        public void CopyTo(Sonos destination)
        {
            destination.Name = this.Name;
        }
    }
}
