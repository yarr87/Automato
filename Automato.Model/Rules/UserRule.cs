using Automato.Model.HomeStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// A rule related to a user
    /// </summary>
    public class UserRule : BaseRuleDefinition
    {
        public override RuleType RuleType
        {
            get { return RuleType.User; }
        }

        public UserState UserState { get; set; }
    }
}
