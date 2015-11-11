namespace Todo.Api.Links
{
    /// <summary>
    /// Refers to a resource that can be used to edit the link's context.
    /// </summary>
    public class DeleteLink : Link
    {
        public const string Relation = "delete";

        public DeleteLink(string href, string method = "DELETE")
            : base(Relation, href, method)
        {
        }
    }
}