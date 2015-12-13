using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Actions
{
    public interface IRuleActionRunner
    {
        Task ExecuteActionAsync(IRuleAction action);
    }

    public interface IRuleActionRunner<T> where T : IRuleAction
    {
        Task ExecuteActionAsync(T action);
    }
}
