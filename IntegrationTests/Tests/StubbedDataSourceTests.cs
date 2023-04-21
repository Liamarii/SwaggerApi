namespace IntegrationTests.Tests;

public class StubbedDataSourceTests
{

    private readonly IServiceCollection _services = new ServiceCollection();

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Get_DatabaseReturnsUsers_ExpectedUsersAreReturned(int userCount)
    {
        //Arrange
        IList<User> users = new Fixture().CreateMany<User>(userCount).ToList();
        Task<IList<User>?> userData = Task.FromResult<IList<User>?>(users);

        _services.AddScoped<IUsersDb>(provider =>
        {
            return new StubbedUserDb(getStubbedData: userData);
        });

        WebApplicationFactory<Startup>? webApplicationFactory = CustomWebApplicationFactory.CreateWebApplicationFactory(_services);
        using var httpClient = webApplicationFactory.CreateDefaultClient();

        //Act
        HttpResponseMessage? response = await httpClient.GetAsync("Users/Get");

        //Assert
        response.Should().BeSuccessful();

        List<User>? responseBody = await response.Content.ReadFromJsonAsync<List<User>>();
        responseBody.Should().BeEquivalentTo(users.ToList());
    }
}

public class MoreStubbedDataSourceTests
{
    [Fact]
    public async Task Get_DatabaseReturnsException_InternalServerErrorIsReturned()
    {
        //Arrange
        IList<User> users = new Fixture().CreateMany<User>(5).ToList();
        Task<IList<User>?> userData = Task.FromResult<IList<User>?>(users);

        IServiceCollection services = new ServiceCollection();

        services.AddScoped<IUsersDb>(provider =>
        {
            return new StubbedUserDb(getStubbedData: new Exception());
        });

        WebApplicationFactory<Startup>? webApplicationFactory = CustomWebApplicationFactory.CreateWebApplicationFactory(services);
        using var httpClient = webApplicationFactory.CreateDefaultClient();

        //Act
        HttpResponseMessage? response = await httpClient.GetAsync("Users/Get");

        //Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}