using Automato.Integration;
using Automato.Logic;
using Automato.Logic.Rules;
using Automato.Logic.Stores;
using Automato.Model;
using Automato.Model.Messages;
using Automato.Model.Rules;
using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Automato.Web.Controllers
{
    public class RuleController : ApiController
    {
        [HttpGet]
        [Route("api/rules")]
        public IHttpActionResult GetRules()
        {
            var ruleStore = new RuleStore();
            var rules = ruleStore.GetAll();
            return Ok(rules);
        }

        [HttpPost]
        [Route("api/rules")]
        public IHttpActionResult SaveRule(Rule rule)
        {
            var ruleStore = new RuleStore();
            ruleStore.Save(rule);
            return Ok(rule);
        }

        [Route("api/rules/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteById(string id)
        {
            new RuleStore().DeleteById(id);

            return Ok();
        }
    }
}