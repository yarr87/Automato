using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    public interface IActionable
    {
        IEnumerable<BaseRuleAction> Actions { get; set; }
    }
}
