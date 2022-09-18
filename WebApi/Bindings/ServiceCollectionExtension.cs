using Microsoft.Extensions.DependencyInjection;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Bindings
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBindings(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IUsersDb, UsersDb>()
                .AddScoped<IUsersService, UsersService>();
        }
    }
}
