using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Todo.Api.Filters;
using Todo.Api.Models;

namespace Todo.Api.Controllers
{
    [ValidateModel]
    public class TodoController : ApiController
    {
        private readonly IToDoRepository _repository;

        public TodoController(IToDoRepository repository)
        {
            _repository = repository;
        }

        // GET api/todo
        public IHttpActionResult Get()
        {
            var all = _repository.GetAll();
            return Ok(all);
        }

        // GET api/todo/5
        public IHttpActionResult Get(int id)
        {
            var todo = _repository.Get(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // POST api/todo
        public IHttpActionResult Post(ToDo todo)
        {
            var addedTodo = _repository.Add(todo);
            return CreatedAtRoute("DefaultApi", new { id = addedTodo.Id }, addedTodo);
        }

        // PUT api/todo/5
        public IHttpActionResult Put(int id, ToDo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest("Route doesn't match todo Id");
            }


            todo.Id = id;
            bool updateResult = _repository.Update(todo);
            if (!updateResult)
            {
                return BadRequest(String.Format("Can't update todo with id {0}", id));
            }

            return Ok();
        }

        // DELETE api/todo/5
        public void Delete(int id)
        {
            _repository.Remove(id);
        }
    }
}
