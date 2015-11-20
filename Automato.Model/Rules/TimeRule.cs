using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// A rule related to the time of day
    /// </summary>
    public class TimeRule : BaseRuleDefinition
    {
        public override RuleType RuleType
        {
            get { return RuleType.Time; }
        }

        /// <summary>
        /// Start time of the trigger.  For point-in-time rules, Start == End.  For "After x" rules, starttime = x.
        /// For "before x" rules, Start = midnight
        /// </summary>
        public TimeSpan Start { get; set; }

        /// <summary>
        /// End time of the trigger.  For point-in-time rules, Start = End.  For "After x" rules, endtime = 11:59pm.
        /// For "before x" rules, end = x
        /// </summary>
        public TimeSpan End { get; set; }

        // TODO: handle these cases
        // Mondays, after 6pm
        // Weekends between 10 and 2
        // Any day after 4pm
        // etc
    }
}
