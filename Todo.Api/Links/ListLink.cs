namespace Todo.Api.Links
{
    /// <summary>
    /// Refers to a resource that can be used to list the link's context.
    /// </summary>
    public class ListLink : Link
    {
        public const string Relation = "list";

        public ListLink(string href, string method = "GET")
            : base(Relation, href, method)
        {
        }
    }
}