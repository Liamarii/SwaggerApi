namespace WebApi.Services
{
    public class DadJokesService : IDadJokesService
    {
        readonly HttpClient _httpClient;
        readonly ILogger _logger;

        public DadJokesService(HttpClient httpClient, ILogger<DadJokesService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetDadJoke()
        {
            try
            {
                _logger.LogInformation("Attempting to get a dad joke");

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("");

                if(!httpResponseMessage.IsSuccessStatusCode)
                {
                    throw new UnsuccessfulResponseException($"{nameof(GetDadJoke)}: returned a status code of {httpResponseMessage.StatusCode} which is not successful");
                }

                string? responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseContent.Trim()))
                {
                    throw new UnsuccessfulResponseException($"{nameof(GetDadJoke)}: the returned response content was null or an empty string");
                }

                DadJoke? dadJoke = JsonConvert.DeserializeObject<DadJoke>(responseContent);

                if(string.IsNullOrEmpty(dadJoke?.Joke?.Trim()))
                {
                    throw new SerializationException($"{nameof(GetDadJoke)}: Unable to deserialize the service response.");
                }

                return dadJoke.Joke;   
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get a dad jFileNameoke: {e.message}", e.Message); //parameterized logging is better than interpolation as you can use the data for log filtering otherwise its just all a string.
                throw;
            }
        }
    }
}
