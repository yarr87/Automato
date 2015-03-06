using Automato.Web.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Automato.Web.Controllers
{
    public class DeviceStateController : ApiController
    {
        private readonly Lazy<IHubContext> DeviceStateHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<DeviceStateHub>());

        [HttpPost]
        [Route("api/devices/{internalName}/state/{state}")]
        public IHttpActionResult UpdateState(string internalName, string state)
        {
            DeviceStateHub.Value.Clients.All.broadcastStateUpdate(new
            {
                deviceInternalName = internalName,
                state = state
            });

            return Ok();
        }
    }
}