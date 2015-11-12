using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using Todo.Api.Controllers;
using Todo.Api.Models;

namespace Todo.Api.Test
{
    [TestClass]
    public class ApiDescriptionControllerShould
    {
        [TestMethod]
        public void ReturnApiDescriptionLinks()
        {
            ApiDescriptionController controller = new ApiDescriptionController();
            MockHelpers.SetupMockUrl(controller);
            var response = controller.Get() as OkNegotiatedContentResult<ApiEntry>;
            response.Content._links.Count.ShouldEqual(3);
            response.Content._links.Any(x => x.rel == "self").ShouldBeTrue();
            response.Content._links.Any(x => x.rel == "list").ShouldBeTrue();
            response.Content._links.Any(x => x.rel == "create").ShouldBeTrue();
        }
    }
}
