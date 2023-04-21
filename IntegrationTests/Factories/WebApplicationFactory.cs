namespace IntegrationTests.Factories;

public static class CustomWebApplicationFactory
{
    public static WebApplicationFactory<Startup> CreateWebApplicationFactory(IServiceCollection? serviceDescriptors = null)
    {
        return new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                if (serviceDescriptors != null)
                {
                    builder.ConfigureServices(services =>
                    {
                        foreach (ServiceDescriptor descriptor in serviceDescriptors)
                        {
                            services.Replace(descriptor);
                        }
                    });
                }
            });
    }
}