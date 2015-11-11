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
        public string Href { get; private set; }

        [DataMember]
        public string Rel { get; private set; }

        [DataMember]
        public string Method { get; private set; }

        public Link(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }

        public override string ToString()
        {
            return Href;
        }
    }
}
