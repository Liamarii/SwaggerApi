using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    internal interface IUsersDb
    {
        public Task<IList<User>?> Get();

        public Task<IList<User>?> Get(string forename, string surname);

        public Task<User?> Get(Guid guid);

        public Task Insert(UserDto userDto);
    }
}