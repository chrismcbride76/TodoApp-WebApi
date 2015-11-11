using System.Collections.Generic;

namespace Todo.Api.Models
{
    public abstract class Resource
    {
        public List<Link> Links { get; private set; }

        public Resource()
        {
            Links = new List<Link>();
        }

        public void AddLink(Link link)
        {
            Links.Add(link);
        }
    }
}