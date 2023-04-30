namespace WebApi.Data
{
    public interface IUsersDb
    {
        public Task<IList<User>?> Get();

        public Task<IList<User>?> Get(string forename, string surname);

        public Task<User?> Get(Guid guid);

        public Task<User?> Insert(UserDto userDto);
    }
}