using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Http.Routing;
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
        Mock<IToDoRepository> _mockRepository = new Mock<IToDoRepository>();
        [TestMethod]
        public void AddTodo()
        {

            TodosController controller = new TodosController(_mockRepository.Object);
            SetupMockUrlHelper(controller);

            ToDo todo = new ToDo
            {
                deadlineUtc = DateTime.UtcNow,
                completed = false,
                task = "Some task"
            };

            _mockRepository.Setup(x => x.Add(todo)).Callback(() => todo.id = 1).Returns(todo);

            var response = controller.Post(todo) as CreatedAtRouteNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.RouteName.ShouldEqual("DefaultApi");
            response.RouteValues["id"].ShouldEqual(response.Content.id);
            response.Content.ShouldEqual(todo);

            _mockRepository.Verify(x => x.Add(todo), Times.Once());
        }

        [TestMethod]
        public void GetTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object);

            ToDo todo = new ToDo
            {
                id = 3,
                deadlineUtc = DateTime.UtcNow,
                completed = false,
                task = "Some task"
            };

            _mockRepository.Setup(x => x.Get(todo.id)).Returns(todo);

            var response = controller.Get(todo.id) as OkNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.Content.ShouldEqual(todo);
        }

        [TestMethod]
        public void GetAllTodos()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

            var allTodos = new List<ToDo>
            {
                new ToDo {completed = true, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))}, // deadline was yesterday, but its already completed
                new ToDo {completed = false, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))},// overdue
                new ToDo {completed = false, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))}, // not due until tomorrow
                new ToDo {completed = true, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
            };
            mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get() as OkNegotiatedContentResult<IEnumerable<ToDo>>;
            response.ShouldNotBeNull();
            response.Content.ShouldEqual(allTodos);
        }

        [TestMethod]
        public void GetOverdueTodos()
        {
            TodosController controller = new TodosController(_mockRepository.Object);

            var allTodos = new List<ToDo>
            {
                new ToDo {completed = true, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))}, // deadline was yesterday, but its already completed
                new ToDo {completed = false, deadlineUtc = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))},// overdue
                new ToDo {completed = false, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))}, // not due until tomorrow
                new ToDo {completed = true, deadlineUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(1))},  // not due until tomorrow
            };
            _mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get(true) as OkNegotiatedContentResult<IEnumerable<ToDo>>;
            response.ShouldNotBeNull();
            response.Content.Count().ShouldEqual(1);
            response.Content.Single().ShouldEqual(allTodos[1]);
        }

        [TestMethod]
        public void ReturnNotFoundIfCantFindTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object);

            var response = controller.Get(1) as NotFoundResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutIdDoesntMatchTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object);

            var response = controller.Put(5, new ToDo {id = 4}) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutCantUpdate()
        {
            TodosController controller = new TodosController(_mockRepository.Object);

            _mockRepository.Setup(x => x.Update(It.IsAny<ToDo>())).Returns(false);

            var response = controller.Put(5, new ToDo { id = 5 }) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
            _mockRepository.Verify(x => x.Update(It.IsAny<ToDo>()), Times.Once);
        }

        [TestMethod]
        public void UpdateTodoOnPut()
        {
            TodosController controller = new TodosController(_mockRepository.Object);

            var todo = new ToDo
            {
                id = 5,
                task = "My task"
            };

            var updatedTodo = new ToDo
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
        public void DeleteTodo()
        {
            TodosController controller = new TodosController(_mockRepository.Object);

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
    }
}
