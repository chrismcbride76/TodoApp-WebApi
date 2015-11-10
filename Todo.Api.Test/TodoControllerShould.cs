using System;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;
using Todo.Api.Controllers;
using Todo.Api.Models;

namespace Todo.Api.Test
{
    [TestClass]
    public class TodoControllerShould
    {
        [TestMethod]
        public void AddTodo()
        {
            var mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            ToDo todo = new ToDo
            {
                DeadlineUtc = DateTime.UtcNow,
                IsCompleted = false,
                Task = "Some task"
            };

            mockRepository.Setup(x => x.Add(todo)).Returns(todo);

            var response = controller.Post(todo) as CreatedAtRouteNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.RouteName.ShouldEqual("DefaultApi");
            response.RouteValues["Id"].ShouldEqual(response.Content.Id);
            response.Content.ShouldEqual(todo);

            mockRepository.Verify(x => x.Add(todo), Times.Once());
        }
    }
}
