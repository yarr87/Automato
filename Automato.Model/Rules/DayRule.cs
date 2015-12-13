using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    /// <summary>
    /// A rule related to the day of the week
    /// </summary>
    public class DayRule : BaseRuleDefinition
    {
        public override RuleType RuleType
        {
            get { return RuleType.Day; }
        }

        /// <summary>
        /// The days that apply to this rule
        /// </summary>
        public List<DayOfWeek> Days { get; set; }
    }
}
