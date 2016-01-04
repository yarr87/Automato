using Automato.Model.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Stores
{
    public class RuleStore : BaseStore<Rule>
    {
        protected override Func<Rule, string> SortExpr
        {
            get
            {
                return (r) => r.Name;
            }
        }

        public IEnumerable<Rule> GetActive()
        {
            var rules = GetAll();

            return rules.Where(r => !r.IsDisabled);
        }

        public void Save(Rule rule)
        {
            using (var session = Context.DocumentStore.Value.OpenSession())
            {
                // Client sends empty string for new devices (null breaks stuff), but we need to send null to the db
                // to generate the correct id
                if (string.IsNullOrWhiteSpace(rule.Id))
                {
                    rule.Id = null;

                    session.Store(rule);
                }
                else
                {
                    // Don't overwrite existing stats on save
                    var existingRule = session.Load<Rule>(rule.Id);

                    if (existingRule != null)
                    {
                        existingRule.Name = rule.Name;
                        existingRule.Description = rule.Description;
                        existingRule.IsDisabled = rule.IsDisabled;
                        existingRule.Actions = rule.Actions;
                        existingRule.RuleDefinitions = rule.RuleDefinitions;
                    }
                    else
                    {
                        throw new Exception(string.Format("Saving rule with id {0} but that id wasn't found in the database", rule.Id));
                    }
                }

                session.SaveChanges();
            }
        }        
    }
}
