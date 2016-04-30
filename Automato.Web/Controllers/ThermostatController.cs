using Automato.Integration;
using Automato.Logic;
using Automato.Logic.Rules;
using Automato.Logic.Stores;
using Automato.Model;
using Automato.Model.Messages;
using Automato.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Automato.Web.Controllers
{
    public class ThermostatController : ApiController
    {
        [HttpGet]
        [Route("api/thermostats")]
        public async Task<IHttpActionResult> GetThermostats()
        {
            var thermostats = await new ThermostatStore().GetAllWithState();
            return Ok(thermostats);
        }

        [HttpPost]
        [Route("api/thermostats")]
        public IHttpActionResult Save(Thermostat thermostat)
        {
            new ThermostatStore().Save(thermostat);
            return Ok(thermostat);
        }

        [Route("api/thermostats/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteById(string id)
        {
            new ThermostatStore().DeleteById(id);

            return Ok();
        }

        [Route("api/thermostats/{id}/temperature")]
        [HttpPost]
        public async Task<IHttpActionResult> SetThermostatTemp(string id, TemperatureModel model)
        {
            await new TemperatureHandler().SetTemperature(id, model.Temperature.ToString());

            return Ok();
        }
    }
}