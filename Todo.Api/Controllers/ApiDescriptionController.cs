﻿using System.Web.Http;
using Todo.Api.Links;
using Todo.Api.Models;

namespace Todo.Api.Controllers
{
    /// <summary>
    /// Controller for the entry point into the RESTful API
    /// </summary>
    public class ApiDescriptionController : ApiController
    {
        public IHttpActionResult Get()
        {
            var entry = new ApiEntry();

            var todoUrl = Url.Link("DefaultApi", new { controller = "todos" });
            entry.AddLink(new SelfLink(Url.Link("api", new { controller = "ApiDescription" })));
            entry.AddLink(new ListLink(todoUrl));
            entry.AddLink(new CreateLink(todoUrl));
            return Ok(entry);
        }
    }
}
