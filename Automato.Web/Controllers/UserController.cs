using Automato.Integration;
using Automato.Logic;
using Automato.Logic.Rules;
using Automato.Model;
using Automato.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Automato.Web.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/users")]
        public IHttpActionResult GetUsers()
        {
            var userStore = new UserStore();
            var users = userStore.GetAll();
            return Ok(users);
        }

        [HttpPost]
        [Route("api/users")]
        public IHttpActionResult SaveUser(User user)
        {
            var userStore = new UserStore();
            userStore.AddOrEditUser(user);
            return Ok(user);
        }

        /// <summary>
        /// React to multiple state updates in a batch.  Broadcast to all clients.  This happens in response to a device's
        /// state being updated either manually or via openHab.
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/presence")]
        public async Task<IHttpActionResult> UpdateUserPresence(IEnumerable<UserPresenceUpdate> updates)
        {
            var userStore = new UserStore();
            userStore.UpdateUserPresence(updates);

            await new RulesManager().ProcessUserPresenceUpdates(updates.Where(u => !u.IsInitializationOnly));

            return Ok();
        }

        [Route("api/users/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteById(string id)
        {
            new UserStore().DeleteById(id);

            return Ok();
        }
    }
}