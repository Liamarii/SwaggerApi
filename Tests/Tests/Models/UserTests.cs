using System;
using WebApi.Models;
using Xunit;

namespace Tests.Models
{
    public class UserTests
    {
        [Fact]
        public void ToUserFromUserDto_OutputsExpectedUser()
        {
            //Arrange
            UserDto userDto = new()
            {
                Forename = "Tobey",
                Surname = "Maguire",
                Age = 47
            };

            //Act
            User user = User.ToUser(userDto);

            //Assert
            Assert.Equal(user?.Forename, userDto.Forename);
            Assert.Equal(user?.Surname, userDto.Surname);
            Assert.Equal(user?.Age, userDto.Age);
            Assert.True( user?.Id is Guid);
        }
    }
}
