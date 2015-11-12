using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using System.Web.Http.Routing;
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
        readonly Mock<IToDoRepository> _mockRepository = new Mock<IToDoRepository>();

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
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            SetupMockUrlHelper(controller);

            TodoModel todo = new TodoModel
            {
                DeadlineUtc = DateTime.UtcNow,
                Completed = false,
                Task = "Some task"
            };

            _mockRepository.Setup(x => x.Add(todo)).Returns(todo);

            var response = controller.Post(todo) as CreatedAtRouteNegotiatedContentResult<TodoRepresentation>;
            response.ShouldNotBeNull();
            response.RouteName.ShouldEqual("DefaultApi");
            response.RouteValues["id"].ShouldEqual(response.Content.id);
            response.Content.deadlineUtc.ShouldEqual(todo.DeadlineUtc);

            _mockRepository.Verify(x => x.Add(todo), Times.Once());
        }

        [TestMethod]
        public void GetTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            TodoModel todo = new TodoModel
            {
                Id = 3,
                DeadlineUtc = DateTime.UtcNow,
                Completed = false,
                Task = "Some task"
            };

            _mockRepository.Setup(x => x.Get(todo.Id)).Returns(todo);

            var response = controller.Get(todo.Id) as OkNegotiatedContentResult<TodoRepresentation>;
            response.ShouldNotBeNull();
            response.Content.id.ShouldEqual(todo.Id);
        }

        [TestMethod]
        public void GetAllTodos()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            var query = SetupDefaultODataQuery(controller);

            var allTodos = new List<TodoModel>
            {
                new TodoModel {Completed = true, DeadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))}, // deadline was yesterday, but its already completed
                new TodoModel {Completed = false, DeadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))},// overdue
                new TodoModel {Completed = false, DeadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))}, // not due until tomorrow
                new TodoModel {Completed = true, DeadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
            };
            _mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get(query) as OkNegotiatedContentResult<List<TodoRepresentation>>;
            response.ShouldNotBeNull();
            response.Content.Count().ShouldEqual(4);
        }

        [TestMethod]
        public void GetOverdueTodos()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);
            var query = SetupDefaultODataQuery(controller);
            var allTodos = new List<TodoModel>
            {
                new TodoModel {Completed = true, DeadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))}, // deadline was yesterday, but its already completed
                new TodoModel {Id = 5, Completed = false, DeadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))},// overdue
                new TodoModel {Completed = false, DeadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))}, // not due until tomorrow
                new TodoModel {Completed = true, DeadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
            };
            _mockRepository.Setup(x => x.GetAll()).Returns(allTodos);


            var response = controller.Get(query, true) as OkNegotiatedContentResult<List<TodoRepresentation>>;
            response.ShouldNotBeNull();
            response.Content.Count.ShouldEqual(1);
            response.Content.Single().id.ShouldEqual(5);
        }

        [TestMethod]
        public void ReturnNotFoundIfCantFindTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            var response = controller.Get(1) as NotFoundResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutIdDoesntMatchTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            var response = controller.Put(5, new TodoModel {Id = 4}) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutCantUpdate()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            _mockRepository.Setup(x => x.Update(It.IsAny<TodoModel>())).Returns(false);

            var response = controller.Put(5, new TodoModel { Id = 5 }) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
            _mockRepository.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
        }

        [TestMethod]
        public void UpdateTodoOnPut()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            var todo = new TodoModel
            {
                Id = 5,
                Task = "My task"
            };

            var updatedTodo = new TodoModel
            {
                Id = 5,
                Task = "Updated task"
            };

            _mockRepository.Setup(x => x.Update(updatedTodo)).Returns(true);

            var response = controller.Put(todo.Id, updatedTodo) as OkResult;
            response.ShouldNotBeNull();

            _mockRepository.Verify(x => x.Update(updatedTodo), Times.Once);
        }

        [TestMethod]
        public void DeleteTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object, Mapper.Engine);

            _mockRepository.Setup(x => x.Remove(1));

            controller.Delete(1);
            _mockRepository.Verify(x => x.Remove(1), Times.Once);
        }

        private static void SetupMockUrlHelper(TodosController controller)
        {
            string locationUrl = "http://location/";

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;
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
    }
}
