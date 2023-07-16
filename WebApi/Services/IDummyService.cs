using System.Net.Http.Headers;

namespace WebApi.Services
{
    public interface IDummyService
    {
        public Task<HttpRequestHeaders?> GetOutboundHeaders();
        public Task<HttpRequestHeaders?> GetInboundHeaders();
    }
}