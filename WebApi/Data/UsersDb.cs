using WebApi.Models;
using System;
using System.Linq;
using WebApi.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public class UsersDb : IUsersDb
    {
        private readonly UserDbContext _context;
        private readonly int _randomDelay;

        public UsersDb(UserDbContext userDbContext)
        {
            _randomDelay = new Random().Next(0, 3000);
            _context = userDbContext; 
        }

        public virtual async Task<IList<User>?> Get()
        {
            try
            {
                await Task.Delay(_randomDelay);
                return _context?.Users?.ToList();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when attempting to get all users", e);
            }
        }

        public async Task<IList<User>?> Get(string forename, string surname)
        {
            try
            {
                await Task.Delay(_randomDelay);
                return _context?.Users?
                .Where(user => string.Equals(user.Forename, forename, StringComparison.CurrentCultureIgnoreCase)
                && string.Equals(user.Surname, surname, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            catch (Exception e)
            {
                throw new DatabaseException($"Error when attempting to get all users by forname {forename} and surname {surname}", e);
            }
        }

        public async Task<User?> Get(Guid guid)
        {
            try
            {
                await Task.Delay(_randomDelay);
                return _context?.Users?
                .Where(user => user.Id == guid)
                .FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new DatabaseException($"Error when attempting to get all users by guid {guid}", e);
            }
        }

        public async Task Insert(UserDto userDto)
        {
            try
            {
                await Task.Delay(_randomDelay);

                User user = User.UserDtoToUser(userDto);

                if (_context.Users != null && !_context.Users.Contains(user))
                {
                    _context?.Users?.Add(user);
                    _context?.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when attempting to insert new user", e);
            }
        }
    }
}
