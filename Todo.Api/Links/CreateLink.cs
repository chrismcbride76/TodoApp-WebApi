namespace Todo.Api.Links
{
    /// <summary>
    /// Refers to a resource that can be used to list the link's context.
    /// </summary>
    public class CreateLink : Link
    {
        public const string Relation = "create";

        public CreateLink(string href, string method = "POST")
            : base(Relation, href, method)
        {
        }
    }
}