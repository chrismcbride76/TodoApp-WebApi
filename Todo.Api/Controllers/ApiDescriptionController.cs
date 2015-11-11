using System.Web.Http;
using Todo.Api.Links;
using Todo.Api.Models;

namespace Todo.Api.Controllers
{
    /// <summary>
    /// Controller for the entry point into the RESTful API
    /// </summary>
    public class ApiDescriptionController : ApiController
    {
        [Route("~/api")]
        public IHttpActionResult Get()
        {
            var entry = new ApiEntry();

            var todoUrl = Url.Link("DefaultApi", new { controller = "todo" });
            entry.AddLink(new SelfLink(Url.Link("Api", new { controller = "apidescription" })));
            entry.AddLink(new EditLink(todoUrl));
            entry.AddLink(new DeleteLink(todoUrl));
            return Ok(entry);
        }
    }
}
