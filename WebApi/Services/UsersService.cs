using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Services;

namespace WebApi.Models
{
    public class UsersService : IUsersService
    {
        private readonly IUsersDb _usersDb;

        public UsersService(IUsersDb usersDb) => _usersDb = usersDb;

        public async Task<User> Insert(UserDto user)
        {
            HttpResponseMessage? httpResponseMessage = _usersDb.Insert(user);
            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(responseContent) ?? new User();
        }

        public async Task<User> Get(Guid guid)
        {
            HttpResponseMessage? httpResponseMessage = _usersDb.Get(guid)!;
            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(responseContent) ?? new User();
        }

        public async Task<IList<User>> Get(string forename, string surname)
        {
            HttpResponseMessage httpResponseMessage = _usersDb.Get(forename, surname);
            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IList<User>>(responseContent) ?? new List<User>();
        }

        public async Task<IList<User>> Get()
        {
            HttpResponseMessage httpResponseMessage = _usersDb.Get();
            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IList<User>>(responseContent) ?? new List<User>();
        }
    }
}
