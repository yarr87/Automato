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
    }
}
