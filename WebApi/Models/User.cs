using System;

namespace WebApi.Models
{
    public class User
    {
        public Guid Id { get; init; }

        public string? Forename { get; init; }

        public string? Surname { get; init; }

        public int? Age { get; init; }

        public static User ToUser(UserDto userDto)
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                Forename = userDto.Forename,
                Surname = userDto.Surname,
                Age = userDto.Age
            }; 
        }
    }
}
