using Automato.Logic.Stores;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic
{
    public class SceneStore : BaseStore<Scene>
    {
        protected override Func<Scene, string> SortExpr
        {
            get
            {
                return (r) => r.Name;
            }
        }
    }
}
