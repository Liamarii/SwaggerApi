namespace IntegrationTests.Tests;

public class RealDataSourceTests : IClassFixture<StubbedTestFixture>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public RealDataSourceTests(StubbedTestFixture testFixture) => _factory = testFixture.Factory;

    /// <summary>
    /// This test just gets all the users from the in memory database to check the connection works
    /// The reason this is being run twice with the not used parameter to to check that the database is not being populated again on each run
    /// Each test has its own fixture data indicated by the iclass fixture attribute above.
    /// I also use fluent assertions because everyone .Should().Be().
    /// </summary>
    /// <param name="testRun"></param>
    /// <returns></returns>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Get_ReturnsUsersCount_MatchesDatabaseUsersCount(int testRun)
    {
        // Arrange
        #pragma warning disable IDE0059 // Unnecessary assignment of a value
        int testRunCount = testRun;
        #pragma warning restore IDE0059 // Unnecessary assignment of a value

        using var httpClient = _factory.CreateDefaultClient();

        // Act
        HttpResponseMessage response = await httpClient.GetAsync("Users/Get");

        // Assert
        response.Should().BeSuccessful();

        List<User>? responseBody = await response.Content.ReadFromJsonAsync<List<User>>();
        responseBody.Should().NotBeNull().And.HaveCount(4);
    }
}

public class MoreRealDataSourceTests : IClassFixture<StubbedTestFixture>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public MoreRealDataSourceTests(StubbedTestFixture testFixture) => _factory = testFixture.Factory;

    /// <summary>
    /// The reason there is this additional class is to test the collection fixture .
    /// </summary>
    /// <param name="testRun"></param>
    /// <returns></returns>
    [Fact]
    public async Task Get_ReturnsUsersCount_MatchesDatabaseUsersCount()
    {
        // Arrange            
        using var httpClient = _factory.CreateDefaultClient();

        // Act
        HttpResponseMessage response = await httpClient.GetAsync("Users/Get");

        // Assert
        response.Should().BeSuccessful();

        List<User>? responseBody = await response.Content.ReadFromJsonAsync<List<User>>();
        responseBody.Should().NotBeNull().And.HaveCount(4);
    }
}