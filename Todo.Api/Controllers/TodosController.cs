using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData.Query;
using AutoMapper;
using Todo.Api.Filters;
using Todo.Api.Models;

namespace Todo.Api.Controllers
{
    [ValidateModel]
    public class TodosController : ApiController
    {
        private readonly IToDoRepository _repository;
        private readonly IMappingEngine _mapper;

        public TodosController(IToDoRepository repository, IMappingEngine mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET api/todos
        public IHttpActionResult Get(ODataQueryOptions<TodoModel> options, bool? overdue = null)
        {
            IEnumerable<TodoModel> todos = _repository.GetAll();
            if (overdue.HasValue)
            {
                todos = overdue == false
                    ? todos.Where(x => !x.completed && x.deadlineUtc >= DateTime.UtcNow)
                    : todos.Where(x => !x.completed && x.deadlineUtc < DateTime.UtcNow);
            }

            IEnumerable<TodoModel> results = (IEnumerable<TodoModel>)options.ApplyTo(todos.AsQueryable(), new ODataQuerySettings());

            var representation = results.Select(_mapper.Map<TodoModel, TodoRepresentation>).ToList();

            return Ok(representation);
        }

        // GET api/todos/5
        public IHttpActionResult Get(int id)
        {
            var todo = _repository.Get(id);
            if (todo == null)
            {
                return NotFound();
            }

            var representation = _mapper.Map <TodoRepresentation>(todo);

            return Ok(representation);
        }

        // POST api/todos
        public IHttpActionResult Post(TodoModel todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo can't be null");
            }

            var addedTodo = _repository.Add(todo);
            var representation = _mapper.Map<TodoRepresentation>(addedTodo);

            return CreatedAtRoute("DefaultApi", new {representation.id }, representation);
        }

        // PUT api/todos/5
        public IHttpActionResult Put(int id, TodoModel todo)
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
            if (_repository.Delete(id))
            {
                return; 
            }
            
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}
