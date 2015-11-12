using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Todo.Api.Enrichers;
using Todo.Api.Models;

namespace Todo.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enrichers are used to add hypermedia links to the model representations
            config.MessageHandlers.Add(new EnrichingHandler());
            config.AddResponseEnrichers(
                    new TodoResponseEnricher()
                );

            Mapper.CreateMap<TodoModel, TodoRepresentation>();

            var builder = new ContainerBuilder();
            builder.RegisterType<InMemoryTodoRepository>().As<IToDoRepository>().SingleInstance();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterInstance(Mapper.Engine);
            
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "Api",
              routeTemplate: "api",
              defaults: new { controller = "ApiDescription"}
          );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
