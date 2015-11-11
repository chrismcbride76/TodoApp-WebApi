
namespace Todo.Api.Links
{
    /// <summary>
    /// Conveys an identifier for the link's context.
    /// </summary>
    public class SelfLink : Link
    {
        public const string Relation = "self";

        public SelfLink(string href, string method = "GET")
            : base(Relation, href, method)
        {
        }
    }
}
