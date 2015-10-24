using Automato.Logic;
using Automato.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Automato.Web.Controllers
{
    public class TagController : ApiController
    {
        [Route("api/tags")]
        public IEnumerable<Tag> Get()
        {
            return new TagStore().GetTags();
        }

        //[Route("api/tags/{id}")]
        //public Device GetById(int id)
        //{
        //    return new DeviceStore().GetDeviceById(id);
        //}

        [Route("api/tags")]
        [HttpPost]
        public IHttpActionResult AddOrEditTag([FromBody] Tag tag)
        {
            new TagStore().AddOrEditTag(tag);

            return Ok(tag);
        }

        [Route("api/tags/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteById(string id)
        {
            new TagStore().DeleteById(id);

            return Ok();
        }
    }
}
