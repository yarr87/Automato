using Automato.Model.Rules;
using Raven.Client.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Stores
{
    public class RuleStore : BaseStore<Rule>
    {
        protected override Func<Rule, string> SortExpr
        {
            get
            {
                return (r) => r.Name;
            }
        }

        public IEnumerable<Rule> GetActive()
        {
            var rules = GetAll();

            return rules.Where(r => !r.IsDisabled);
        }
    }
}
