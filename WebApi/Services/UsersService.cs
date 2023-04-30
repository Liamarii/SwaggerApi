namespace WebApi.Models
{
    public class UsersService : IUsersService
    {
        private readonly IUsersDb _usersDb;

        public UsersService(IUsersDb usersDb) => _usersDb = usersDb;

        public async Task<User?> Insert(UserDto user)
        {
           return await _usersDb.Insert(user);
        }

        public async Task<User?> Get(Guid guid)
        {
            return await _usersDb.Get(guid)!;
        }

        public async Task<IList<User>?> Get(string forename, string surname)
        {
            return await _usersDb.Get(forename, surname);
        }

        public async Task<IList<User>?> Get()
        {
            return await _usersDb.Get();
        }
    }
}
