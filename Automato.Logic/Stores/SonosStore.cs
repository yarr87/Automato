using Automato.Integration;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Stores
{
    public class SonosStore : BaseStore<Sonos>
    {
        protected override Func<Sonos, string> SortExpr
        {
            get
            {
                return (r) => r.Name;
            }
        }

        /// <summary>
        /// Returns all sonoses with playlists/favorites
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Sonos>> GetAllExtended()
        {
            var sonoses = base.GetAll();
            var sonosService = new SonosHttpService();

            foreach (var sonos in sonoses)
            {
                sonos.Favorites = await sonosService.GetFavorites(sonos.Name);
            }

            return sonoses;
        }
    }
}
