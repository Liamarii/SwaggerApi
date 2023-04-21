using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Threading;
using WebApi.Bindings;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static readonly ReaderWriterLockSlim _databaseLock = new();

        private static bool _databasePopulated = false;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBindings();
            services.AddControllers();
            services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("MyDb"));
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Users Service API", Version = "v1" });
                x.EnableAnnotations();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            PopulateDatabase(serviceProvider.GetService<UserDbContext>()!);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseSwagger()
                .UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("../swagger/v1/swagger.json", "UsersService");
                    x.EnableTryItOutByDefault();
                })
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }

        public static void PopulateDatabase(UserDbContext context)
        {
            if (!_databasePopulated)
            {
                lock (_databaseLock)
                {
                    if (!_databasePopulated)
                    {
                        for (int i = 0; i < new DummyData().Users.Count; i++)
                        {
                            context.Add(new DummyData().Users[i]);
                        }
                        context.SaveChanges();
                        _databasePopulated = true;
                    }
                }
            }
        }

        public IServiceCollection AddBindings(IServiceCollection serviceCollection)
        {
            serviceCollection.Clear();
            serviceCollection.AddTransient<IUsersDb, UsersDb>();
            serviceCollection.AddTransient<IUsersService, UsersService>();
            return serviceCollection;
        }
    }
}
