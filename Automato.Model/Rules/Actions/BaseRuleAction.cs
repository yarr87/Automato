using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Model.Rules.Actions
{
    [JsonConverter(typeof(BaseRuleActionConverter))]
    public abstract class BaseRuleAction : IRuleAction
    {
        /// <summary>
        /// Type of action (needed to serialize the base class to its implementation, and to know what component to render on the frontend
        /// </summary>
        public abstract RuleActionType ActionType { get; }

        /// <summary>
        /// Ensures rule actions are serialized to the correct subclass
        /// </summary>
        private class BaseRuleActionConverter : JsonSubclassConverter<BaseRuleAction>
        {
            protected override BaseRuleAction Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
            {
                var ruleAction = jObject.Value<string>("actionType");

                if (ruleAction == "Light")
                {
                    return new LightRuleAction();
                }
                else if (ruleAction == "EmailAsText")
                {
                    return new EmailAsTextRuleAction();
                }
                else if (ruleAction == "Temperature")
                {
                    return new TemperatureRuleAction();
                }
                else
                {
                    throw new Exception("Unrecognized rule action " + ruleAction);
                }
            }
        }
    }
}
