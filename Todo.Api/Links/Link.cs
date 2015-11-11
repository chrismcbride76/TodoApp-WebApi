using System.Runtime.Serialization;

namespace Todo.Api.Links
{
    /// <summary>
    /// A base class for relation links
    /// </summary>
    [DataContract]
    public class Link
    {
        [DataMember]
        public string href { get; private set; }

        [DataMember]
        public string rel { get; private set; }

        [DataMember]
        public string method { get; private set; }

        public Link(string rel, string href, string method)
        {
            this.rel = rel;
            this.href = href;
            this.method = method;
        }

        public override string ToString()
        {
            return href;
        }
    }
}
