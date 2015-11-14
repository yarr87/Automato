using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules
{
    [JsonConverter(typeof(BaseRuleDefinitionConverter))]
    public abstract class BaseRuleDefinition : IRuleDefinition
    {
        public abstract RuleType RuleType { get; }

        /// <summary>
        /// Flag to tell if this rule is triggered.  When true, responds to an event (ie, a light turning on, someone coming home).  When
        /// false, just responds to the current state.
        /// </summary>
        public bool IsTriggered { get; set; }

        /// <summary>
        /// Ensures rule definitions are serialized to the correct subclass
        /// </summary>
        private class BaseRuleDefinitionConverter : JsonSubclassConverter<BaseRuleDefinition>
        {
            protected override BaseRuleDefinition Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
            {
                var ruleType = jObject.Value<string>("ruleType");

                if (ruleType == "Light")
                {
                    return new LightRule();
                }
                else if (ruleType == "User")
                {
                    return new UserRule();
                }
                else if (ruleType == "Time")
                {
                    return new TimeRule();
                }
                else
                {
                    throw new Exception("Unrecognized rule type " + ruleType);
                }
            }
        }
    }
}
