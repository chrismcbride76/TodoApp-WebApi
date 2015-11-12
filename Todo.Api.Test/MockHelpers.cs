using System.Web.Http;
using System.Web.Http.Routing;
using Moq;

namespace Todo.Api.Test
{
    public class MockHelpers
    {
        public static void SetupMockUrl(ApiController controller)
        {
            string locationUrl = "http://location/";

            var mockUrlHelper = new Mock<UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;
        }
    }
}
