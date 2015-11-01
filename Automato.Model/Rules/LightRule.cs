using Automato.Model.HomeStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// A rule related to a light
    /// </summary>
    public class LightRule : BaseRuleDefinition
    {
        public override RuleType RuleType
        {
            get { return RuleType.Light; }
        }

        public LightState LightState { get; set; }
    }
}
