using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Todo.Api.Controllers;

namespace Todo.Api.Test
{
    [TestClass]
    public class TodoControllerShould
    {
        [TestMethod]
        public void SetLocationHeaderOnPost()
        {
            TodoController controller = new TodoController();

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string baseUrl = "http://baseUrl/";

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(baseUrl);
            controller.Url = mockUrlHelper.Object;

            Models.ToDo todo = new Models.ToDo
            {
                DeadlineUtc = DateTime.UtcNow,
                IsCompleted = true,
                Task = "Some task"
            };
           // var response = controller.Post(todo);

            //response.Headers



        }
    }
}
