using Moq;
using WebApi.Data;
using WebApi.Models;
using Xunit;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Tests.Services
{
    public sealed class UsersServiceTests
    {
        private readonly UsersService _sut;
        private readonly Mock<IUsersDb> _usersDb;
        private readonly User _userOne = new()
        {
            Forename = "Bertram",
            Surname = "Gilfoyle",
            Id = Guid.NewGuid(),
            Age = 38
        };
        private readonly User _userTwo = new()
        {
            Forename = "Jìan",
            Surname = "Yáng",
            Id = Guid.NewGuid(),
            Age = 25
        };

        public UsersServiceTests()
        {
            _usersDb = new Mock<IUsersDb>();
            _sut = new UsersService(_usersDb.Object);
        }

        #region Get
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetAllUsers_FindsMultipleUsers_OutputsExpectedNumberOfUsers(int returnedUsers)
        {
            //Arrange
            List<User> users = new();
            for (int i = 0; i < returnedUsers; i++)
            {
                users.Add(new User());
            }
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(users))
            };

            _usersDb
                .Setup(x => x.Get())
                .Returns(response);

            //Act
            var result = await _sut.Get();

            //Assert
            Assert.True(result.Count == returnedUsers);
        }

        [Fact]
        public async Task GetAllUsers_FindsUsers_OutputExpectedUserDetails()
        {
            //Arrange
            List<User> users = new()
            {
                _userOne,
                _userTwo
            };
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(users))
            };

            var userOneSerialized = JsonConvert.SerializeObject(users[0]);
            var userTwoSerialized = JsonConvert.SerializeObject(users[1]);

            _usersDb
                .Setup(x => x.Get())
                .Returns(response);

            //Act
            var result = await _sut.Get();
            var resultOneSerialized = JsonConvert.SerializeObject(result[0]);
            var resultTwoSerialized = JsonConvert.SerializeObject(result[1]);

            //Assert
            Assert.Matches(userOneSerialized, resultOneSerialized);
            Assert.Matches(userTwoSerialized, resultTwoSerialized);
            Assert.DoesNotMatch(userOneSerialized, resultTwoSerialized);
        }
        #endregion

        #region GetById
        [Fact]
        public async Task GetUsersById_FindsUserById_OutputsExpectedUserDetails()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_userOne))
            };

            _usersDb
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns(response);

            //Act
            var result = await _sut.Get(Guid.NewGuid());
            var serializedResult = JsonConvert.SerializeObject(result);

            //Assert
            Assert.Matches(serializedResult, JsonConvert.SerializeObject(_userOne));
        }
        #endregion

        #region GetByName
        [Fact]
        public async Task GetUsersByName_FindsOneUserByName_OutputsExpectedUserDetails()
        {
            //Arrange
            List<User> users = new()
            {
                _userOne
            };
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(users))
            };
            var serializedUser = JsonConvert.SerializeObject(users[0]);

            _usersDb
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(response);

            //Act
            var result = await _sut.Get("notah","otdog");
            var serializedResult = JsonConvert.SerializeObject(result[0]);


            //Assert
            Assert.Equal(serializedUser, serializedResult);
            Assert.True(result.Count == 1);
        }

        [Fact]
        public async Task GetUsersByName_FindsTwoUsersByName_OutputsExpectedUserDetails()
        {
            //Arrange
            List<User> users = new()
            {
                _userOne,
                _userTwo
            };
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(users))
            };
            var serializedUserOne = JsonConvert.SerializeObject(users[0]);
            var serializedUserTwo = JsonConvert.SerializeObject(users[1]);

            _usersDb
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(response);

            //Act
            var result = await _sut.Get("notah", "otdog");
            var serializedResultOne = JsonConvert.SerializeObject(result[0]);
            var serializedResultTwo = JsonConvert.SerializeObject(result[1]);

            //Assert
            Assert.Equal(serializedUserOne, serializedResultOne);
            Assert.Equal(serializedUserTwo, serializedResultTwo);
            Assert.True(result.Count == 2);
        }
        #endregion

        #region Insert
        [Fact]
        public async Task InsertNewUser_AddsUserDetails_ReturnsExpectedUserDetails()
        {
            //Arrange
            string userSerialized = JsonConvert.SerializeObject(_userOne);
            HttpResponseMessage response = new(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_userOne))
            };
            _usersDb
                .Setup(x => x.Insert(It.IsAny<UserDto>()))
                .Returns(response);

            //Act
            var result = await _sut.Insert(new UserDto());
            var resultSerialized = JsonConvert.SerializeObject(result);

            //Assert
            Assert.Matches(userSerialized, resultSerialized);
        }
        #endregion
    }
}