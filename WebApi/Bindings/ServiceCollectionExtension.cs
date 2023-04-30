namespace WebApi.Bindings
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBindings(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUsersDb, UsersDb>();
            serviceCollection.AddTransient<IUsersService, UsersService>();
            return serviceCollection;
        }
    }
}
