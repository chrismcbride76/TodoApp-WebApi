using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using Todo.Api.Links;

namespace Todo.Api.Enrichers
{
    /// <summary>
    /// "Enriches" todorespresentation responses with additional hyperlinks (HATEOAS)
    /// </summary>
    public class TodoResponseEnricher : IResponseEnricher
    {
        public bool CanEnrich(HttpResponseMessage response)
        {
            var content = response.Content as ObjectContent;

            return content != null
                && (content.ObjectType == typeof(TodoRepresentation) || content.ObjectType == typeof(List<TodoRepresentation>));
        }

        public HttpResponseMessage Enrich(HttpResponseMessage response)
        {
            TodoRepresentation todo;

            var urlHelper = response.RequestMessage.GetUrlHelper();

            if (response.TryGetContentValue(out todo))
            {
                Enrich(todo, urlHelper);
                return response;
            }

            List<TodoRepresentation> representations;
            if (response.TryGetContentValue(out representations))
            {
                representations.ToList().ForEach(p => Enrich(p, urlHelper));
            }

            return response;
        }

        private void Enrich(TodoRepresentation todo, UrlHelper url)
        {
            var selfUrl = url.Link("DefaultApi", new { controller = "todos", todo.id });
            todo.AddLink(new SelfLink(selfUrl));
            todo.AddLink(new EditLink(selfUrl));
            todo.AddLink(new DeleteLink(selfUrl));
        }
    }
}