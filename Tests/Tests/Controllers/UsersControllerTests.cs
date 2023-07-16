using Moq;
using WebApi.Controllers;
using WebApi.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Bogus;
using FluentAssertions;

namespace Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController _sut;
        private readonly Mock<IUsersService> _usersService;
        private readonly Mock<IDadJokesService> _dadJokesService;
        private readonly UserDto _validUserDto;
        private readonly User _validUser;
        private readonly Faker _faker;

        public UsersControllerTests()
        {
            _faker = new();
            _usersService = new Mock<IUsersService>();
            _dadJokesService = new Mock<IDadJokesService>();
            _sut = new UsersController(_usersService.Object, _dadJokesService.Object);
            _validUserDto = new UserDto()
            {
                Forename = _faker.Person.FirstName,
                Surname = _faker.Person.LastName,
                Age = _faker.Random.Number(0, 99)
            };
            _validUser = User.UserDtoToUser(_validUserDto);
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
            ActionResult<IList<User>>? actionResult = await _sut.Get();

            //Assert
            actionResult?.Result.Should().BeAssignableTo<NotFoundResult>();
            actionResult?.Value.Should().BeNull();
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
            ActionResult<IList<User>>? actionResult = await _sut.Get();

            //Assert
            actionResult.Should().NotBeNull();
            actionResult?.Result?.Should().BeAssignableTo<NotFoundResult>();
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
            ActionResult<IList<User>>? actionResult = await _sut.Get();

            //Assert
            actionResult.Should().NotBeNull();
            actionResult?.Result.Should().BeAssignableTo<ObjectResult>();

            ObjectResult? objectResult = actionResult?.Result as ObjectResult;
            objectResult?.Value.Should().BeAssignableTo<IList<User>>();

            IList<User>? resultData = objectResult?.Value as IList<User>;
            users.Should().BeEquivalentTo(resultData);
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
            ActionResult<IList<User>>? actionResult = await _sut.Get();

            //Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task Get_ReturnsExeption_OutputsException()
        {
            //Arrange
            string exceptionMessage = "Exception message";

            _usersService
                .Setup(x => x.Get())
                .ThrowsAsync(new Exception(exceptionMessage));

            //Act
            ActionResult<IList<User>>? actionResult = await _sut.Get();

            //Assert
            actionResult.Should().NotBeNull();
            actionResult?.Result.Should().BeAssignableTo<ObjectResult>();
            
            ObjectResult? objectResult = actionResult?.Result as ObjectResult;
            objectResult?.Value.Should().BeAssignableTo<ProblemDetails>();
            
            ProblemDetails? problemDetails = objectResult?.Value as ProblemDetails;
            problemDetails?.Detail.Should().BeEquivalentTo(exceptionMessage);
        }
        #endregion
    }
}
