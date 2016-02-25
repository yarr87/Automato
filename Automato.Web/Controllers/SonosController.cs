using Automato.Integration;
using Automato.Logic;
using Automato.Logic.Rules;
using Automato.Logic.Stores;
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
    public class SonosController : ApiController
    {
        [HttpGet]
        [Route("api/sonos")]
        public IHttpActionResult GetSonos()
        {
            var sonos = new SonosStore().GetAll();
            return Ok(sonos);
        }

        [HttpPost]
        [Route("api/sonos")]
        public IHttpActionResult Save(Sonos sonos)
        {
            new SonosStore().Save(sonos);
            return Ok(sonos);
        }
        
        //[Route("api/sonos/{name}/{command}")]
        //[HttpPost]
        //public async Task<IHttpActionResult> SendCommand(string name, string command, [FromUri] string parameter = "")
        //{
        //    await new SonosHttpService().SendCommand(name, command, parameter);

        //    return Ok();
        //}

        [Route("api/sonos/{name}/favorite")]
        [HttpPost]
        public async Task<IHttpActionResult> PlayFavorite(string name, [FromUri] string favorite)
        {
            await new SonosHttpService().PlayFavorite(name, favorite);

            return Ok();
        }

        [Route("api/sonos/{name}/play")]
        [HttpPost]
        public async Task<IHttpActionResult> Play(string name)
        {
            await new SonosHttpService().Play(name);

            return Ok();
        }

        [Route("api/sonos/{name}/pause")]
        [HttpPost]
        public async Task<IHttpActionResult> Pause(string name)
        {
            await new SonosHttpService().Pause(name);

            return Ok();
        }

        [Route("api/sonos/{name}/next")]
        [HttpPost]
        public async Task<IHttpActionResult> Next(string name)
        {
            await new SonosHttpService().Next(name);

            return Ok();
        }

        [Route("api/sonos/{name}/favorites")]
        [HttpGet]
        public async Task<IEnumerable<string>> GetFavorites(string name)
        {
            return await new SonosHttpService().GetFavorites(name);
        }

        [Route("api/sonos/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteById(string id)
        {
            new SonosStore().DeleteById(id);

            return Ok();
        }
    }
}