using Moq;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly UsersService _sut;
        private readonly Mock<IUsersDb> _usersDb; 

        public UsersServiceTests()
        {
            _usersDb = new Mock<IUsersDb>();
            _sut = new UsersService(_usersDb.Object);
        } 

        #region Get
        //TODO: Finish writing tests for the user service Get method
        #endregion

        #region GetById
        //TODO: Write tests for the user service GetById method
        #endregion

        #region GetByName
        //TODO: Write tests for the user service GetByName method
        #endregion

        #region Insert
        //TODO: Write tests for the user service Insert method
        #endregion      
    }
}