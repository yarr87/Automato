using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    public interface IRuleAction
    {
        RuleActionType ActionType { get; }
    }
}
