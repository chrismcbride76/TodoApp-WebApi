//using System.Web.Http;
//using Todo.Api.Links;
//using Todo.Api.Models;

//namespace Todo.Api.Controllers
//{
//    /// <summary>
//    /// Controller for the entry point of the RESTful API
//    /// </summary>
//    public class ApiDescriptionController : ApiController
//    {
//        // GET api/apidescription
//        public IHttpActionResult Get()
//        {
//            var entry = new ApiEntry();

//            var todoUrl = Url.Link("DefaultApi", new { controller = "todo" });
//            entry.AddLink(new SelfLink(Url.Link("DefaultApi", new { controller = "apidescription" })));
//            entry.AddLink(new EditLink(todoUrl));
//            entry.AddLink(new DeleteLink(todoUrl));
//            return Ok(entry);
//        }
//    }
//}
