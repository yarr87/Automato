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
        /// Time to trigger the rule, or start time if it's a range
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End of the range, or null if it's just a point in time
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
