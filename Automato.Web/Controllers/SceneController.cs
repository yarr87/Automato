using Automato.Logic;
using Automato.Logic.Rules;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Automato.Web.Controllers
{
    public class SceneController : ApiController
    {
        [Route("api/scenes")]
        public IEnumerable<Scene> Get()
        {
            return new SceneStore().GetAll();
        }

        [HttpPost]
        [Route("api/scenes")]
        public IHttpActionResult SaveScene(Scene scene)
        {
            var sceneStore = new SceneStore();
            sceneStore.Save(scene);
            return Ok(scene);
        }

        /// <summary>
        /// Test a scene before saving
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/scenes/test")]
        public async Task<IHttpActionResult> TestScene(Scene scene)
        {
            await new RuleRunner().RunRule(scene);

            return Ok();
        }

        [Route("api/scenes/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteById(string id)
        {
            new SceneStore().DeleteById(id);

            return Ok();
        }

        /// <summary>
        /// Trigger a specific scene by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/scenes/{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> TriggerScene(string id)
        {
            var scene = new SceneStore().GetById(id);

            await new RuleRunner().RunRule(scene);

            return Ok();
        }

        [Route("api/scenes/triggers")]
        public IEnumerable<SceneTrigger> GetTriggers()
        {
            return new SceneTriggerStore().GetAll();
        }

        [Route("api/scenes/triggers")]
        [HttpPost]
        public IHttpActionResult SaveTrigger(IEnumerable<SceneTrigger> triggers)
        {
            var sceneTriggerStore = new SceneTriggerStore();

            sceneTriggerStore.Save(triggers);

            return Ok(triggers);
        }

        /// <summary>
        /// Trigger a 'SceneTrigger' device, which maps to a single scene to trigger.
        /// This will be called from openHab rules, which trigger whe minimote buttons are pressed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/scenes/trigger/{triggerInternalName}")]
        [HttpPost]
        public async Task<IHttpActionResult> SceneTrigger(string triggerInternalName)
        {
            var trigger = new SceneTriggerStore().GetByInternalName(triggerInternalName);

            if (trigger != null)
            {
                var scene = new SceneStore().GetById(trigger.SceneId);

                await new RuleRunner().RunRule(scene);

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
