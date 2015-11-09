using System;
using System.Net.Http;
using System.Web.Http;
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
    public class TodoControllerShould
    {
        [TestMethod]
        public void SetLocationHeaderOnPost()
        {
            TodoController controller = new TodoController();

            ToDo todo = new ToDo
            {
                DeadlineUtc = DateTime.UtcNow,
                IsCompleted = true,
                Task = "Some task"
            };

            var response = controller.Post(todo) as CreatedAtRouteNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.RouteName.ShouldEqual("DefaultApi");
            response.RouteValues["Id"].ShouldEqual(response.Content.Id);
            response.Content.ShouldEqual(todo);  // TODO: verify response in the body properly
        }

        public void SaveTodoInRepositoryOnPost()
        {
            var mockRepository = new Mock<IToDoRepository>(MockBehavior.Strict);
            TodoController controller = new TodoController(mockRepository.Object);

            ToDo todo = new ToDo
            {
                DeadlineUtc = DateTime.UtcNow,
                IsCompleted = true,
                Task = "Some task"
            };
            //mockRepository.Setup(x => x.CreateNewTodo(todo)).Returns(new Product { Id = 42 });
            Assert.Fail("Still need to add verification");

            var response = controller.Post(todo);


        }
    }
}
