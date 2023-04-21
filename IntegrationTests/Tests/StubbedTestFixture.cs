namespace IntegrationTests.Tests;

public class StubbedTestFixture : ICollectionFixture<WebApplicationFactory<Startup>>
{
    public WebApplicationFactory<Startup> Factory { get; }

    public StubbedTestFixture()
    {
        Factory = new WebApplicationFactory<Startup>();
    }
}