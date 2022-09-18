using System.Net;
using WebApi.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Linq;

namespace WebApi.Data
{
    public class UsersDb : IUsersDb
    {
        private readonly UserDbContext _context;
        public UsersDb(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }

        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_context.Users))
            };
        }

        public HttpResponseMessage Get(string forename, string surname)
        {
            var user = _context?.Users?
                .Where(x => string.Equals(x.Forename, forename, StringComparison.CurrentCultureIgnoreCase)
                && string.Equals(x.Surname, surname, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user))
            };
        }

        public HttpResponseMessage Get(Guid guid)
        {
            User? user = _context?.Users?
                .Where(x => x.Id == guid)
                .FirstOrDefault();
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user))
            };
        }

        public HttpResponseMessage Insert(UserDto userDto)
        {
            User user = User.ToUser(userDto);

            if(_context.Users != null && !_context.Users.Contains(user))
            {
                _context?.Users?.Add(user);
                _context?.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user))
            };
        }
    }
}
