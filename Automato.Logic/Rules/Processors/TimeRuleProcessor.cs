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
    /// Tells if the given time rule is active.  This means we are within the time range defined, or if it's a single point-in-time,
    /// we are after it but within the rule-processing step time.
    /// </summary>
    public class TimeRuleProcessor : BaseRuleProcessor<TimeRule>
    {
        public override bool IsRuleActive(TimeRule rule, HomeState state)
        {
            var currentTime = state.Time.TimeOfDay;

            // Point in time rule
            if (rule.TimeRuleType == TimeRuleType.PointInTime)
            {
                // We need to have a window of time so a point in time doesn't trigger much later.  For example a rule is "at 6pm if Jeff is home, turn on a light", but
                // I get home at 8pm.  It shouldn't trigger that rule.  The 5 minutes should be less than or equal to the frequency we run the rules via job.
                var endOfWindow = rule.Start.Add(TimeSpan.FromMinutes(5));

                return rule.Start <= currentTime && endOfWindow >= currentTime;
            }
            // Range rule
            else if (rule.TimeRuleType == TimeRuleType.After)
            {
                return rule.Start < currentTime;
            }
            else if (rule.TimeRuleType == TimeRuleType.Before)
            {
                return rule.End > currentTime;
            }
            else // Between
            {
                // Single-day range
                if (rule.Start < rule.End)
                {
                    return rule.Start <= currentTime && rule.End >= currentTime;
                }
                // Day-spanning range
                else
                {
                    return rule.Start <= currentTime || rule.End >= currentTime;
                }
            }
        }
    }
}
