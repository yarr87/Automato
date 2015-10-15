using Automato.Integration;
using Automato.Logic;
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
    public class DevicesController : ApiController
    {
        /// <summary>
        /// Get a list of all devices.  Includes current state.
        /// </summary>
        /// <returns></returns>
        [Route("api/devices")]
        public async Task<IEnumerable<Device>> Get()
        {
            return await new DeviceStore().GetDevices();
        }

        [Route("api/devices/{id}")]
        public Device GetById(int id)
        {
            return new DeviceStore().GetDeviceById(id);
        }

        [Route("api/devices")]
        [HttpPost]
        public IHttpActionResult AddOrEditComponent([FromBody] Device component)
        {
            new DeviceStore().AddOrEditDevice(component);

            return Ok(component);
        }

        [Route("api/devices/{name}/{command}")]
        [HttpPost]
        public async Task<IHttpActionResult> SendCommand(string name, string command)
        {
            await new OpenHabRestService().SendCommand(name, command);

            return Ok();
        }

        [Route("api/devices/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteById(string id)
        {
            new DeviceStore().DeleteById(id);

            return Ok();
        }
    }
}
