using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;
using Todo.Api.Controllers;
using Todo.Api.Models;

namespace Todo.Api.Test
{
    [TestClass]
    public class TodosControllerShould
    {
        private readonly Mock<IToDoRepository> _mockRepository = new Mock<IToDoRepository>();

        [TestInitialize]
        public void Initialize()
        {
            Mapper.CreateMap<TodoModel, TodoRepresentation>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Mapper.Reset();
        }

        [TestMethod]
        public void AddTodo()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            MockHelpers.SetupMockUrl(controller);

            TodoModel todo = new TodoModel
            {
                deadlineUtc = DateTime.UtcNow,
                completed = false,
                task = "Some task"
            };

            _mockRepository.Setup(x => x.Add(todo)).Returns(todo);

            var response = controller.Post(todo) as CreatedAtRouteNegotiatedContentResult<TodoRepresentation>;
            response.ShouldNotBeNull();
            response.RouteName.ShouldEqual("DefaultApi");
            response.RouteValues["id"].ShouldEqual(response.Content.id);
            AreEquivalent(todo, response.Content).ShouldBeTrue();

            _mockRepository.Verify(x => x.Add(todo), Times.Once());
        }

        [TestMethod]
        public void ReturnBadRequestIfEmptyBodyOnPost()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            var response = controller.Post(null) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void GetTodo()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            TodoModel todo = new TodoModel
            {
                id = 3,
                deadlineUtc = DateTime.UtcNow,
                completed = false,
                task = "Some task"
            };

            _mockRepository.Setup(x => x.Get(todo.id)).Returns(todo);

            var response = controller.Get(todo.id) as OkNegotiatedContentResult<TodoRepresentation>;
            response.ShouldNotBeNull();
            AreEquivalent(todo, response.Content).ShouldBeTrue();
        }

        [TestMethod]
        public void GetAllTodos()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            var query = SetupDefaultODataQuery(controller);

            var allTodos = new List<TodoModel>
            {
                new TodoModel {completed = true, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))}, // deadline was yesterday, but its already completed
                new TodoModel {completed = false, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))},// overdue
                new TodoModel {completed = false, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))}, // not due until tomorrow
                new TodoModel {completed = true, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
            };
            _mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get(query) as OkNegotiatedContentResult<List<TodoRepresentation>>;
            response.ShouldNotBeNull();
            response.Content.Count.ShouldEqual(4);
            AreEquivalent(allTodos, response.Content).ShouldBeTrue();
        }

        [TestMethod]
        public void GetOverdueTodos()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            var query = SetupDefaultODataQuery(controller);
            var allTodos = new List<TodoModel>
            {
                new TodoModel {completed = true, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))}, // deadline was yesterday, but its already completed
                new TodoModel {id = 5, completed = false, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))},// overdue
                new TodoModel {completed = false, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))}, // not due until tomorrow
                new TodoModel {completed = true, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
            };
            _mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get(query, true) as OkNegotiatedContentResult<List<TodoRepresentation>>;
            response.ShouldNotBeNull();
            response.Content.Count.ShouldEqual(1);
            response.Content.Single().id.ShouldEqual(5);
            AreEquivalent(allTodos[1], response.Content.Single()).ShouldBeTrue(); 
        }

        [TestMethod]
        public void GetTodosThatAreNotOverdue()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            var query = SetupDefaultODataQuery(controller);
            var allTodos = new List<TodoModel>
            {
                new TodoModel {completed = true, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))}, // deadline was yesterday, but its already completed
                new TodoModel {id = 5, completed = false, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))},// overdue
                new TodoModel {completed = false, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))}, // not due until tomorrow
                new TodoModel {completed = false, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
                new TodoModel {completed = true, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
            };
            _mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get(query, false) as OkNegotiatedContentResult<List<TodoRepresentation>>;
            response.ShouldNotBeNull();
            response.Content.Count.ShouldEqual(2);
            AreEquivalent(allTodos[2], response.Content[0]).ShouldBeTrue(); 
            AreEquivalent(allTodos[3], response.Content[1]).ShouldBeTrue(); 
        }

        [TestMethod]
        public void ReturnNotFoundIfCantFindTodo()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            var response = controller.Get(1) as NotFoundResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutIdDoesntMatchTodo()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            var response = controller.Put(5, new TodoModel {id = 4}) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutCantUpdate()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            _mockRepository.Setup(x => x.Update(It.IsAny<TodoModel>())).Returns(false);

            var response = controller.Put(5, new TodoModel { id = 5 }) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
            _mockRepository.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
        }

        [TestMethod]
        public void UpdateTodoOnPut()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            var todo = new TodoModel
            {
                id = 5,
                task = "My task"
            };

            var updatedTodo = new TodoModel
            {
                id = 5,
                task = "Updated task"
            };

            _mockRepository.Setup(x => x.Update(updatedTodo)).Returns(true);

            var response = controller.Put(todo.id, updatedTodo) as OkResult;
            response.ShouldNotBeNull();

            _mockRepository.Verify(x => x.Update(updatedTodo), Times.Once);
        }

        [TestMethod]
        public void ReturnBadRequestIfEmptyBodyOnPut()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            var response = controller.Put(1, null) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void DeleteTodo()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            _mockRepository.Setup(x => x.Delete(1)).Returns(true);

            controller.Delete(1);
        }

        [TestMethod]
        public void ThrowNotFoundIfCantDeleteTodo()
        {
            var controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            _mockRepository.Setup(x => x.Delete(1)).Returns(false);

            try
            {
                controller.Delete(1);
                Assert.Fail();
            }
            catch (HttpResponseException e)
            {
                e.Response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);
            }
        }

        private ODataQueryOptions<TodoModel> SetupDefaultODataQuery(ApiController controller)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<TodoModel>("TodoModel");
            var opts = new ODataQueryOptions<TodoModel>(new ODataQueryContext(modelBuilder.GetEdmModel(), typeof(TodoModel)), request);
            controller.Request = request;
            return opts;
        }

        /// <summary>
        /// Used to determine if the model and representations are equivalent
        /// </summary>
        /// <param name="model"></param>
        /// <param name="representation"></param>
        /// <returns></returns>
        private static bool AreEquivalent(TodoModel model, TodoRepresentation representation)
        {
            return
                model.id == representation.id &&
                model.completed == representation.completed &&
                model.deadlineUtc == representation.deadlineUtc &&
                model.moreDetails == representation.moreDetails &&
                model.task == representation.task;
        }

        /// <summary>
        /// Used to determine if the model and representations are equivalent
        /// </summary>
        /// <param name="models"></param>
        /// <param name="representations"></param>
        /// <returns></returns>
        private static bool AreEquivalent(IReadOnlyList<TodoModel> models, IReadOnlyList<TodoRepresentation> representations)
        {
            if (models.Count != representations.Count)
                return false;

            for (int i = 0; i < models.Count; i++)
            {
                if (!AreEquivalent(models[i], representations[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
