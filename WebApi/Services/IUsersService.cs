using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IUsersService
    {
        public Task<IList<User>?> Get();
        public Task<User?> Get(Guid guid);
        public Task<IList<User>?> Get(string forename, string surname);
        public Task Insert(UserDto user);
    }
}
