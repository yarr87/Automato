using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// Types of time rules
    /// </summary>
    public enum TimeRuleType
    {
        /// <summary>
        /// At a specific time ("at 6pm")
        /// </summary>
        PointInTime,
        /// <summary>
        /// Before a specific time ("before 6pm") (after midnight)
        /// </summary>
        Before,
        /// <summary>
        /// After a specific time ("after 6pm") (until midnight)
        /// </summary>
        After,
        /// <summary>
        /// Between two times ("between 4pm and 6pm")
        /// </summary>
        Between
    }
}
