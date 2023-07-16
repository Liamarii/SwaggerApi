namespace WebApi.Services
{
    public interface IDadJokesService
    {
        public Task<string> GetDadJoke();
    }
}