using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;
using WebApi.Support;
using Xunit;

namespace Tests.Models
{
    public class UserDtoTests
    {
        [Fact]
        public void UserDto_FullUserModel_IsValid()
        {
            //Arrange
            UserDto userDto = new()
            {
                Forename = "Charlie",
                Surname = "Day",
                Age = 32
            };

            //Act
            bool validModel = ModelValidator.ValidateModel(userDto).Count == 0;

            //Assert
            Assert.True(validModel);
        }

        [Theory]
        [InlineData("Charlie", "Day", null)]
        [InlineData("Charlie", "", 30)] 
        [InlineData("", "Day", 30)]
        [InlineData("", "", null)] 
        public void UserDto_PartialUserModel_IsInvalid(string? forename, string? surname, int? age)
        {
            //Arrange
            UserDto userDto = new()
            {
                Forename = forename,
                Surname = surname,
                Age = age
            };

            //Act
            bool validModel = ModelValidator.ValidateModel(userDto).Count == 0;

            //Assert
            Assert.False(validModel);
        }

        [Fact]
        public void UserDto_InvalidModel_ReturnsExpectedErrors()
        {
            //Arrange
            UserDto userDto = new();

            //Act
            List<ValidationResult>? result = ModelValidator.ValidateModel(userDto);

            //Assert
            Assert.Contains(result, x => x.ErrorMessage == "The Forename field is required.");
            Assert.Contains(result, x => x.ErrorMessage == "The Surname field is required.");
            Assert.Contains(result, x => x.ErrorMessage == "The Surname field is required.");
        }
    }
}
