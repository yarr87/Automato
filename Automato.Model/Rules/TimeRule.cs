﻿using System;
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
        /// Type of time rule that is defined (ex: at a specific time, or between two times)
        /// </summary>
        public TimeRuleType TimeRuleType { get; set; }

        /// <summary>
        /// Start time of the trigger.  For point-in-time rules, Start == End.  For "After x" rules, starttime = x.
        /// For "before x" rules, Start = midnight
        /// </summary>
        public TimeSpan Start { get; set; }

        /// <summary>
        /// End time of the trigger.  For point-in-time rules, Start = End.  For "After x" rules, endtime = midnight.
        /// For "before x" rules, end = x
        /// </summary>
        public TimeSpan End { get; set; }

        /// <summary>
        /// Flag to indicate start time is dynamic (sunrise or sunset).  Put in a separate field so I don't have to worry
        /// about migrating existing rules.
        /// </summary>
        public SpecialTimeType SpecialStart { get; set; }

        /// <summary>
        /// Flag to indicate start time is dynamic (sunrise or sunset).  Put in a separate field so I don't have to worry
        /// about migrating existing rules.
        /// </summary>
        public SpecialTimeType SpecialEnd { get; set; }

        // TODO: handle these cases
        // Mondays, after 6pm
        // Weekends between 10 and 2
        // Any day after 4pm
        // etc
    }
}
