using System.Collections.Generic;
using System.Web.Http;
using Todo.Api.Models;

namespace Todo.Api.Controllers
{
    public class TodoController : ApiController
    {
        private readonly IToDoRepository _toDoRepository;

        public TodoController(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        // GET api/todo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/todo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/todo
        public IHttpActionResult Post(ToDo todo)
        {

            return CreatedAtRoute("DefaultApi", new { id = todo.Id }, todo);
        }

        // PUT api/todo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/todo/5
        public void Delete(int id)
        {
        }
    }
}
