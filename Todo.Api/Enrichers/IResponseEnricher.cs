using System.Net.Http;

namespace Todo.Api.Enrichers
{
    public interface IResponseEnricher
    {
        bool CanEnrich(HttpResponseMessage response);
        HttpResponseMessage Enrich(HttpResponseMessage response);
    }
}
