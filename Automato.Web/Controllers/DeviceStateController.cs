using Automato.Integration;
using Automato.Model;
using Automato.Web.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Automato.Web.Controllers
{
    public class DeviceStateController : ApiController
    {
        private readonly Lazy<IHubContext> DeviceStateHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<DeviceStateHub>());

        /// <summary>
        /// React to a state update by broadcasting it to all clients.  This happens in response to a device's
        /// state being updated either manually or via openHab.
        /// </summary>
        /// <param name="internalName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/devices/{internalName}/state/{state}")]
        public IHttpActionResult StateUpdated(string internalName, string state)
        {
            // TODO: probably not used, use the batch version below instead

            DeviceStateHub.Value.Clients.All.broadcastStateUpdate(new
            {
                deviceInternalName = internalName,
                state = state
            });

            return Ok();
        }

        /// <summary>
        /// React to multiple state updates in a batch.  Broadcast to all clients.  This happens in response to a device's
        /// state being updated either manually or via openHab.
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/devicestates")]
        public IHttpActionResult StatesUpdated(IEnumerable<DeviceState> updates)
        {
            DeviceStateHub.Value.Clients.All.broadcastStateUpdates(updates);

            return Ok();
        }

        /// <summary>
        /// Update a device's state
        /// </summary>
        /// <param name="internalName"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/devices/{internalName}/set/state/{state}")]
        public async Task<IHttpActionResult> UpdateState(string internalName, string state)
        {
            await new OpenHabRestService().SendCommand(internalName, state);

            //// For now, notifying clients manually.  Eventually would just rely on the auto-update
            //// from openHab/console app, same flow as for manual state changes.
            //DeviceStateHub.Value.Clients.All.broadcastStateUpdate(new
            //{
            //    deviceInternalName = internalName,
            //    state = state
            //});

            return Ok();
        }
    }
}