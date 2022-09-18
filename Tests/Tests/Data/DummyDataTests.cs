using Newtonsoft.Json;
using WebApi.Data;
using WebApi.Models;
using Xunit;

namespace Tests.Data
{
    public class DummyDataTests
    {
        private readonly DummyData _dummyData;

        public DummyDataTests() => _dummyData = new DummyData();

        [Fact]
        public void GetUsersDummyData_ReturnsUserData()
        {
            //Arrange
            User defaultUser = new();

            //Act
            var result = _dummyData.Users;
            var dummyUser = result[0];

            //Assert
            Assert.True(dummyUser.Forename != defaultUser.Forename);
            Assert.True(dummyUser.Surname != defaultUser.Surname);
            Assert.True(dummyUser.Age != defaultUser.Age);
            Assert.True(dummyUser.Id != defaultUser.Id);
            Assert.DoesNotMatch(JsonConvert.SerializeObject(defaultUser), JsonConvert.SerializeObject(dummyUser));
        }
    }
}
