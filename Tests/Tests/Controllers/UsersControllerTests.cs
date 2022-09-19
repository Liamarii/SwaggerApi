using Moq;
using WebApi.Controllers;
using WebApi.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Tests.Controllers
{
    public sealed class UsersControllerTests
    {
        private readonly UsersController _sut;
        private readonly Mock<IUsersService> _usersService;

        public UsersControllerTests()
        {
            _usersService = new Mock<IUsersService>();
            _sut = new UsersController(_usersService.Object);
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
            var actionResult = await _sut.Get() as ObjectResult;
            IList<User>? returnedUsers = actionResult?.Value as IList<User>;

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
            IActionResult? actionResult = await _sut.Get();

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task Get_ReturnsUser_OutputsUserData()
        {
            //Arrange
            List<User> users = new()
            {
                new User()
                {
                    Age = 60,
                    Forename = "Vic",
                    Surname = "Reeves"
                }
            };

            _usersService
                .Setup(x => x.Get())
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get() as OkObjectResult;
            IList<User>? returnedUsers = actionResult?.Value as IList<User>;

            var expected = JsonConvert.SerializeObject(users?.FirstOrDefault());
            var actual = JsonConvert.SerializeObject(returnedUsers?.FirstOrDefault());

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Get_ReturnsUser_OutputsOkResponseType()
        {
            //Arrange
            List<User> users = new()
            {
                new User()
                {
                    Age = 60,
                    Forename = "Vic",
                    Surname = "Reeves"
                }
            };

            _usersService
                .Setup(x => x.Get())
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get() as ObjectResult;

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult);
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
            var expected = JsonConvert.SerializeObject(objectResult);
            var actual = JsonConvert.SerializeObject(await _sut.Get() as ObjectResult);

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
            var actionResult = await _sut.Get(Guid.NewGuid()) as ObjectResult;
            IList<User>? returnedUsers = actionResult?.Value as IList<User>;

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
            IActionResult? actionResult = await _sut.Get(Guid.NewGuid());

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
            OkObjectResult? actionResult = await _sut.Get(Guid.NewGuid()) as OkObjectResult;
            User? returnedUsers = actionResult?.Value as User;

            string? expected = JsonConvert.SerializeObject(user);
            string? actual = JsonConvert.SerializeObject(returnedUsers);

            //Assert
            Assert.Equal(expected, actual);
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
            var actionResult = await _sut.Get(Guid.NewGuid()) as ObjectResult;

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult);
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
            var expected = JsonConvert.SerializeObject(objectResult);
            var actual = JsonConvert.SerializeObject(await _sut.Get(Guid.NewGuid()) as ObjectResult);

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
            var actionResult = await _sut.Get("Bob","Loblaw") as ObjectResult;
            IList<User>? returnedUsers = actionResult?.Value as IList<User>;

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
            IActionResult? actionResult = await _sut.Get("Bob","Loblaw");

            //Assert
            Assert.IsAssignableFrom<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task GetByName_ReturnsUser_OutputsUserData()
        {
            List<User> users = new()
            {
                new User()
                {
                    Age = 60,
                    Forename = "Vic",
                    Surname = "Reeves"
                }
            };
           
            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw") as ObjectResult;
            List<User>? returnedUsers = actionResult?.Value as List<User>;

            string? expected = JsonConvert.SerializeObject(users);
            string? actual = JsonConvert.SerializeObject(returnedUsers);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByName_ReturnsUser_OutputsOkResponseType()
        {
            //Arrange
            List<User> users = new()
            {
                new User()
                {
                    Age = 60,
                    Forename = "Vic",
                    Surname = "Reeves"
                }
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw") as ObjectResult;

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetByName_ReturnsUsers_OutputsUserData()
        {
            List<User> users = new()
            {
                new User()
                {
                    Age = 60,
                    Forename = "Vic",
                    Surname = "Reeves"
                },
                new User()
                {
                    Age = 60,
                    Forename = "Bob",
                    Surname = "Mortimer"
                }
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw") as ObjectResult;
            List<User>? returnedUsers = actionResult?.Value as List<User>;

            string? expected = JsonConvert.SerializeObject(users);
            string? actual = JsonConvert.SerializeObject(returnedUsers);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByName_ReturnsUsers_OutputsOkResponseType()
        {
            //Arrange
            List<User> users = new()
            {
                new User()
                {
                    Age = 60,
                    Forename = "Vic",
                    Surname = "Reeves"
                },
                new User()
                {
                    Age = 60,
                    Forename = "Bob",
                    Surname = "Mortimer"
                }
            };

            _usersService
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(users);

            //Act
            var actionResult = await _sut.Get("Bob", "Loblaw") as ObjectResult;

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult);
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
            var expected = JsonConvert.SerializeObject(objectResult);
            var actual = JsonConvert.SerializeObject(await _sut.Get("Bob", "Loblaw") as ObjectResult);

            //Assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region AddUser
        [Fact]
        public async Task AddUser_ReturnsUser_OutputsUserData()
        {
            //Arrange
            User user = new()
            {
                Age = 60,
                Forename = "Vic",
                Surname = "Reeves"
            };

            _usersService
                .Setup(x => x.Insert(It.IsAny<UserDto>()))
                .ReturnsAsync(user);

            //Act
            var actionResult = await _sut.Insert(new UserDto()) as ObjectResult;
            User? returnedUser = actionResult?.Value as User;

            //Assert
            Assert.Equal(user.Forename, returnedUser?.Forename);
            Assert.Equal(user.Surname, returnedUser?.Surname);
            Assert.Equal(user.Age,returnedUser?.Age);
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
            var actionResult = await _sut.Insert(new UserDto()) as ObjectResult;

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(actionResult);
        }

        [Theory]
        [InlineData("Frank","string")]
        [InlineData("string", "Reynolds")]
        [InlineData("string", "string")]

        public async Task AddUser_DefaultNameValues_OutputBadRequestResponseType(string forename, string surname)
        {
            //Arrange
            UserDto user = new()
            {
                Forename = forename,
                Surname = surname
            };

            //Act
            var actionResult = await _sut.Insert(user);

            //Assert
            Assert.IsAssignableFrom<BadRequestResult>(actionResult);
        }

        [Fact]
        public async Task AddUser_ReturnsExeption_OutputsException()
        {
            //Arrange
            string exceptionMessage = "Exception message";
            ObjectResult? objectResult = new(exceptionMessage)
            {
                StatusCode = 500
            };

            _usersService
                .Setup(x => x.Insert(It.IsAny<UserDto>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            var expected = JsonConvert.SerializeObject(objectResult);
            var actual = JsonConvert.SerializeObject(await _sut.Insert(new UserDto()) as ObjectResult);

            //Act / Assert
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
