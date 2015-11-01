using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Processors
{
    /// <summary>
    /// Tells if the given time rule is active.  This means we are after the time defined in the rule, but within an hour
    /// </summary>
    public class TimeRuleProcessor : BaseRuleProcessor<TimeRule>
    {
        public override bool IsRuleActive(TimeRule rule, Model.HomeStates.HomeState state)
        {
            var timeSpan = state.Time - rule.StartTime;

            // Arbitrarily chose an hour.  Not sure if it's even needed, just make sure it's after the time and if the previous
            // state is before the rule will trigger.
            // TODO: handle time range rules
            return timeSpan.TotalSeconds > 0 && timeSpan.TotalHours <= 1;
        }
    }
}
