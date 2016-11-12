using Automato.Integration;
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
        private async Task<TimeSpan> GetTime(TimeSpan time, SpecialTimeType special)
        {
            // TODO: might be better to just have some external process update the sunrise/sunset times automatically each day.
            // If this api call is slow it could slow down all rule processing...
            if (special == SpecialTimeType.Sunrise)
            {
                var sunrise = await new SunriseSunsetService().GetSunriseToday();
                return sunrise.GetValueOrDefault().TimeOfDay;
            }
            else if (special == SpecialTimeType.Sunset)
            {
                var sunset = await new SunriseSunsetService().GetSunsetToday();
                return sunset.GetValueOrDefault().TimeOfDay;
            }
            else
            {
                return time;
            }
        }
        public override async Task<bool> IsRuleActive(TimeRule rule, HomeState state)
        {
            var currentTime = state.Time.TimeOfDay;
            // Use time from rule, or dynamic sunrise/sunset for current day
            var start = await GetTime(rule.Start, rule.SpecialStart);
            var end = await GetTime(rule.End, rule.SpecialEnd);

            bool isActive;

            // Point in time rule
            if (rule.TimeRuleType == TimeRuleType.PointInTime)
            {
                // We need to have a window of time so a point in time doesn't trigger much later.  For example a rule is "at 6pm if Jeff is home, turn on a light", but
                // I get home at 8pm.  It shouldn't trigger that rule.  The 5 minutes should be less than or equal to the frequency we run the rules via job.
                var endOfWindow = start.Add(TimeSpan.FromMinutes(5));

                isActive = start <= currentTime && endOfWindow >= currentTime;
            }
            // Range rule
            else if (rule.TimeRuleType == TimeRuleType.After)
            {
                isActive = start < currentTime;
            }
            else if (rule.TimeRuleType == TimeRuleType.Before)
            {
                isActive = end > currentTime;
            }
            else // Between
            {
                // Single-day range
                if (start < end)
                {
                    isActive = start <= currentTime && end >= currentTime;
                }
                // Day-spanning range
                else
                {
                    isActive = start <= currentTime || end >= currentTime;
                }
            }

            return isActive;
        }
    }
}
