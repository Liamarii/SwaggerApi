using WebApi.Models;

namespace WebApi.Data
{
    public interface IUsersDb
    {
        public HttpResponseMessage Get();

        public HttpResponseMessage Get(string forename, string surname);

        public HttpResponseMessage Get(Guid guid);

        public HttpResponseMessage Insert(UserDto userDto);
    }
}
