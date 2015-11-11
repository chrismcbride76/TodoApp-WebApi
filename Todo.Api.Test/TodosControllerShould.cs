using System;
using System.Collections.Generic;
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
        [TestMethod]
        public void AddTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);
            SetupMockUrlHelper(controller);

            ToDo todo = new ToDo
            {
                deadlineUtc = DateTime.UtcNow,
                isCompleted = false,
                task = "Some task"
            };

            mockRepository.Setup(x => x.Add(todo)).Callback(() => todo.id = 1).Returns(todo);

            var response = controller.Post(todo) as CreatedAtRouteNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.RouteName.ShouldEqual("DefaultApi");
            response.RouteValues["id"].ShouldEqual(response.Content.id);
            response.Content.ShouldEqual(todo);

            mockRepository.Verify(x => x.Add(todo), Times.Once());
        }

        [TestMethod]
        public void GetTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

            ToDo todo = new ToDo
            {
                id = 3,
                deadlineUtc = DateTime.UtcNow,
                isCompleted = false,
                task = "Some task"
            };

            mockRepository.Setup(x => x.Get(todo.id)).Returns(todo);

            var response = controller.Get(todo.id) as OkNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.Content.ShouldEqual(todo);
        }

        [TestMethod]
        public void GetAllTodos()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

            var allTodos = new List<ToDo>();
            mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get() as OkNegotiatedContentResult<IEnumerable<ToDo>>;
            response.ShouldNotBeNull();
            response.Content.ShouldEqual(allTodos);
        }


        [TestMethod]
        public void ReturnNotFoundIfCantFindTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

            var response = controller.Get(1) as NotFoundResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutIdDoesntMatchTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

            var response = controller.Put(5, new ToDo {id = 4}) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutCantUpdate()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

            mockRepository.Setup(x => x.Update(It.IsAny<ToDo>())).Returns(false);

            var response = controller.Put(5, new ToDo { id = 5 }) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
            mockRepository.Verify(x => x.Update(It.IsAny<ToDo>()), Times.Once);
        }

        [TestMethod]
        public void UpdateTodoOnPut()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

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

            mockRepository.Setup(x => x.Update(updatedTodo)).Returns(true);

            var response = controller.Put(todo.id, updatedTodo) as OkResult;
            response.ShouldNotBeNull();

            mockRepository.Verify(x => x.Update(updatedTodo), Times.Once);
        }

        [TestMethod]
        public void DeleteTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodosController controller = new TodosController(mockRepository.Object);

            mockRepository.Setup(x => x.Remove(1));

            controller.Delete(1);
            mockRepository.Verify(x => x.Remove(1), Times.Once);
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
