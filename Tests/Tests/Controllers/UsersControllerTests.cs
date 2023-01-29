using Moq;
using WebApi.Controllers;
using WebApi.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.Json;
using Bogus;

namespace Tests.Controllers
{
    public sealed class UsersControllerTests
    {
        private readonly UsersController _sut;
        private readonly Mock<IUsersService> _usersService;
        private readonly UserDto _validUserDto;
        private readonly User _validUser;
        private readonly Faker _faker;

        //TODO: The guard tests are missing.

        public UsersControllerTests()
        {
            _faker = new();
            _usersService = new Mock<IUsersService>();
            _sut = new UsersController(_usersService.Object);
            _validUserDto = new UserDto()
            {
                Forename = _faker.Person.FirstName,
                Surname = _faker.Person.LastName,
                Age = _faker.Random.Number(0, 99)
            };
            _validUser = User.ToUser(_validUserDto);
        }

        #region Get
        [Fact]
        public async Task Get_ReturnsNoUsers_OutputsNoData()
        {
            //Arrange
            IList<User>? users = new List<User>();
            _usersService
                .Setup(x => x.Get())
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get();
            IList<User>? returnedUsers = actionResult?.Value;

            //Assert
            Assert.Null(returnedUsers);
        }

        [Fact]
        public async Task Get_ReturnsNoUsers_OutputsNotFoundResponseType()
        {
            //Arrange
            IList<User>? users = new List<User>();
            _usersService
                .Setup(x => x.Get())
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get();

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task Get_ReturnsUser_OutputsUserData()
        {
            //Arrange
            List<User> users = new()
            {
                _validUser
            };

            _usersService
                .Setup(x => x.Get())
                .ReturnsAsync(users);

            //Act
            var result = await _sut.Get();
            var resultData = (result?.Result as ObjectResult)?.Value as IList<User>;

            //Assert
            Assert.Equal(users, resultData);
        }

        [Fact]
        public async Task Get_ReturnsUser_OutputsOkResponseType()
        {
            //Arrange
            List<User> users = new()
            {
                _validUser
            };

            _usersService
                .Setup(x => x.Get())
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get();

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task Get_ReturnsExeption_OutputsException()
        {
            //Arrange
            string exceptionMessage = "Exception message";
            ObjectResult? objectResult = new(exceptionMessage)
            {
                StatusCode = 500
            };

            _usersService
            .Setup(x => x.Get())
            .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            var response = (await _sut.Get()).Result as ObjectResult;
            var expected = JsonSerializer.Serialize(objectResult);
            var actual = JsonSerializer.Serialize(response);

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region GetById
        [Fact]
        public async Task GetById_ReturnsNoUser_OutputsNoData()
        {
            //Arrange
            _usersService
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => null!);

            //Act
            var actionResult = await _sut.Get(Guid.NewGuid());
            var returnedUsers = actionResult?.Value;

            //Assert
            Assert.Null(returnedUsers);
        }

        [Fact]
        public async Task GetById_ReturnsNoUser_OutputsNotFoundResponseType()
        {
            //Arrange
            _usersService
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => null!);

            //Act
            var actionResult = (await _sut.Get(Guid.NewGuid())).Result;

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task GetById_ReturnsUser_OutputsUserData()
        {
            //Arrange
            User user = new()
            {
                Age = 60,
                Forename = "Vic",
                Surname = "Reeves"
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => user);

            //Act
            var result = await _sut.Get(Guid.NewGuid());
            var resultData = (result?.Result as ObjectResult)?.Value as User;

            //Assert
            Assert.Equal(user, resultData);
        }

        [Fact]
        public async Task GetById_ReturnsUser_OutputsOkResponseType()
        {
            //Arrange
            User user = new()
            {
                Age = 60,
                Forename = "Vic",
                Surname = "Reeves"
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => user);

            //Act
            var actionResult = await _sut.Get(Guid.NewGuid());

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetById_ReturnsExeption_OutputsException()
        {
            //Arrange
            string exceptionMessage = "Exception message";
            ObjectResult? objectResult = new(exceptionMessage)
            {
                StatusCode = 500
            };

            _usersService
            .Setup(x => x.Get(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            var response = (await _sut.Get(Guid.NewGuid())).Result as ObjectResult;
            var expected = JsonSerializer.Serialize(objectResult);
            var actual = JsonSerializer.Serialize(response);

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region GetByName
        [Fact]
        public async Task GetByName_ReturnsNoUser_OutputsNoData()
        {
            //Arrange
            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null!);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw");
            IList<User>? returnedUsers = actionResult?.Value;

            //Assert
            Assert.Null(returnedUsers);
        }

        [Fact]
        public async Task GetByName_ReturnsNoUser_OutputsNotFoundResponseType()
        {
            //Arrange
            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null!);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw");

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetByName_ReturnsUser_OutputsUserData()
        {
            List<User> users = new()
            {
                _validUser
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var response = await _sut.Get("aa", "bb");
            var responseData = response.Result as ObjectResult;

            string? expected = JsonSerializer.Serialize(users);
            string? actual = JsonSerializer.Serialize(responseData?.Value);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByName_ReturnsUser_OutputsOkResponseType()
        {
            //Arrange
            List<User> users = new()
            {
                _validUser
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw");

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetByName_ReturnsUsers_OutputsUserData()
        {
            List<User> users = new()
            {
                _validUser,
                _validUser
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var response = await _sut.Get("aa", "bb");
            var responseData = response.Result as ObjectResult;

            string? expected = JsonSerializer.Serialize(users);
            string? actual = JsonSerializer.Serialize(responseData?.Value);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByName_ReturnsUsers_OutputsOkResponseType()
        {
            //Arrange
            List<User> users = new()
            {
                _validUser,
                _validUser
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw");

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetByName_ReturnsExeption_OutputsException()
        {
            //Arrange
            string exceptionMessage = "Exception message";
            ObjectResult? objectResult = new(exceptionMessage)
            {
                StatusCode = 500
            };

            _usersService
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            var response = (await _sut.Get("Bob", "Loblaw")).Result as ObjectResult;

            var expected = JsonSerializer.Serialize(objectResult);
            var actual = JsonSerializer.Serialize(response);

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region PostUser
        [Fact]
        public async Task AddUser_ReturnsUser_OutputsUserData()
        {
            //Arrange
            UserDto userDto = new UserDto()
            {
                Forename = "abc",
                Surname = "def",
                Age = 10
            };

            User user = User.ToUser(userDto);

            _usersService
                .Setup(x => x.Insert(It.IsAny<UserDto>()))
                .ReturnsAsync(user);

            //Act
            var actionResult = await _sut.Post(userDto);
            var returnedUser = (actionResult.Result as ObjectResult)?.Value as User;

            //Assert
            Assert.Equal(user.Forename, returnedUser?.Forename);
            Assert.Equal(user.Surname, returnedUser?.Surname);
            Assert.Equal(user.Age, returnedUser?.Age);
            Assert.True(typeof(Guid) == returnedUser?.Id.GetType());
        }

        [Fact]
        public async Task AddUser_ReturnsUser_OutputsOkResponseType()
        {
            //Arrange
            _usersService
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(() => new User());

            //Act
            var actionResult = await _sut.Post(new UserDto() { Forename = "b", Surname = "a", Age = 10 });

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
        }

        [Theory]
        [InlineData("Frank", "string", 10)]
        [InlineData("string", "Reynolds", 20)]
        [InlineData("string", "string", 30)]

        public async Task AddUser_DefaultNameValues_OutputBadRequestResponseType(string forename, string surname, int age)
        {
            //Arrange
            UserDto user = new()
            {
                Forename = forename,
                Surname = surname,
                Age = age
            };

            //Act
            var response = await _sut.Post(user);

            //Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task AddUser_ReturnsExeption_OutputsException()
        {
            //Arrange
            string exceptionMessage = "Exception message";
            UserDto userDto = new()
            {
                Forename = "abc",
                Surname = "def",
                Age = 10
            };

            ObjectResult? objectResult = new(exceptionMessage)
            {
                StatusCode = 500
            };

            _usersService
                .Setup(x => x.Insert(It.IsAny<UserDto>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            var response = await _sut.Post(userDto);
            var responseData = response.Result as ObjectResult;

            var expected = JsonSerializer.Serialize(objectResult);
            var actual = JsonSerializer.Serialize(responseData);

            //Act / Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
