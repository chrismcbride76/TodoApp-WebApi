using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Todo.Api.Filters;
using Todo.Api.Links;
using Todo.Api.Models;

namespace Todo.Api.Controllers
{
    [ValidateModel]
    public class TodosController : ApiController
    {
        private readonly IToDoRepository _repository;

        public TodosController(IToDoRepository repository)
        {
            _repository = repository;
        }

        // GET api/todos
        [EnableQuery]
        public IHttpActionResult Get(bool overdue = false)
        {
            var results = overdue ? _repository.GetAll().Where(x => !x.completed && DateTime.UtcNow > x.deadlineUtc) : _repository.GetAll();

            return Ok(results);
        }

        // GET api/todos/5
        public IHttpActionResult Get(int id)
        {
            var todo = _repository.Get(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // POST api/todos
        public IHttpActionResult Post(ToDo todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo can't be null");
            }

            var addedTodo = _repository.Add(todo);

            var selfUrl = Url.Link("DefaultApi", new {controller = "todos", id = addedTodo.id});
            addedTodo.AddLink(new SelfLink(selfUrl));
            addedTodo.AddLink(new EditLink(selfUrl));
            addedTodo.AddLink(new DeleteLink(selfUrl));

            return CreatedAtRoute("DefaultApi", new { id = addedTodo.id }, addedTodo);
        }

        // PUT api/todos/5
        public IHttpActionResult Put(int id, ToDo todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo can't be null");
            }

            todo.id = id;
            bool updateResult = _repository.Update(todo);
            if (!updateResult)
            {
                return BadRequest(String.Format("Can't update todo with id {0}", id));
            }

            return Ok();
        }

        // DELETE api/todos/5
        public void Delete(int id)
        {
            _repository.Remove(id);
        }
    }
}
