using Newtonsoft.Json;
using System;
using WebApi.Data;
using WebApi.Models;
using Xunit;

namespace Tests.Data
{
    public sealed class DummyDataTests
    {
        private readonly DummyData _sut;

        public DummyDataTests() => _sut = new DummyData();

        [Fact]
        public void GetUsersDummyData_DoesntReturnDefaultData()
        {
            //Arrange
            User defaultUser = new();

            //Act
            var result = _sut.Users;
            var dummyUser = result[0];

            //Assert
            Assert.DoesNotMatch(JsonConvert.SerializeObject(defaultUser), JsonConvert.SerializeObject(dummyUser));
        }

        [Fact]
        public void GetUsersDummyData_ReturnsFullyPopulatedModels()
        {
            //Arrange
            var users = _sut.Users;

            //Act / Assert
            for (int i = 0; i < users.Count; i++)
            {
                Assert.Multiple(() =>
                {
                    Assert.False(string.IsNullOrEmpty(users[i].Forename),$"dummy data Forename is returning: {users[i].Forename}");
                    Assert.False(string.IsNullOrEmpty(users[i].Surname), $"dummy data Surname is returning: {users[i].Surname}");
                    Assert.False(users[i].Age == null, "dummy data age is  returning null");
                    Assert.False(users[i].Id == Guid.Empty, "dummy data id is returning an empty guid");
                });
            }
        }
    }
}
