namespace WebApi.Services
{
    public interface IUsersService
    {
        public Task<IList<User>?> Get();
        public Task<User?> Get(Guid guid);
        public Task<IList<User>?> Get(string forename, string surname);
        public Task<User?> Insert(UserDto user);
    }
}
