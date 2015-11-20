using Automato.Model.HomeStates;
using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Processors
{
    /// <summary>
    /// Processing day of week rules, tells if we are in a certain date
    /// </summary>
    public class DayRuleProcessor : BaseRuleProcessor<DayRule>
    {
        public override bool IsRuleActive(DayRule rule, HomeState state)
        {
            throw new NotImplementedException();
        }
    }
}
