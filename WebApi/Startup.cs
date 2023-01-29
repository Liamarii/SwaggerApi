using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using WebApi.Bindings;
using WebApi.Data;
using WebApi.Models;

namespace WebApi
{
    // Stryker disable all
    internal sealed class Startup
    {
        public IConfiguration Configuration { get; }

        public IHostEnvironment HostEnvironment { get; private set; }

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
            Configuration = configuration;
        }

        public static void ConfigureServices(IServiceCollection services)
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

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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

        private static void PopulateDatabase(UserDbContext context)
        {
            for (int i = 0; i < new DummyData().Users.Count; i++)
            {
                context.Add(new DummyData().Users[i]);
            }
            context.SaveChanges();
        }
    }
}