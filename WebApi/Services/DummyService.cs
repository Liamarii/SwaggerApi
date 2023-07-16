using System.Net.Http.Headers;

namespace WebApi.Services
{
    public class DummyService : IDummyService
    {
        private readonly HttpClient _httpClient;

        public DummyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpRequestHeaders?> GetOutboundHeaders()
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("");
            return httpResponseMessage?.RequestMessage?.Headers;
        }

        public async Task<HttpRequestHeaders?> GetInboundHeaders()
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("");
            
            return httpResponseMessage?.RequestMessage?.Headers;
        }
    }
}
