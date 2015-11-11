using System.Collections.Generic;
using Todo.Api.Links;

namespace Todo.Api.Models
{
    public abstract class Resource
    {
        public List<Link> _links { get; private set; }

        protected Resource()
        {
            _links = new List<Link>();
        }

        public void AddLink(Link link)
        {
            _links.Add(link);
        }
    }
}