namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        private static readonly ReaderWriterLockSlim _databaseLock = new();

        private static bool _databasePopulated = false;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHeaderPropagation(options =>
            {
                options.Headers.Add("PropagatedHeader");
            });

            services.AddHttpClient<IDadJokesService, DadJokesService>(client =>
            {
                client.DefaultRequestHeaders.Add("X-User-Agent", Configuration.GetValue<string>("RepositoryUrl"));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.BaseAddress = new Uri(Configuration.GetValue<string>("DadJokesUrl"));
            });

            services.AddHttpClient<IDummyService, DummyService>(client => 
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("ExampleSiteUrl"));
            }).AddHeaderPropagation();
            
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
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
            PopulateDatabase(serviceProvider?.GetService<UserDbContext>());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHeaderPropagation();

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseExceptionHandler(applicationBuilder =>
                {
                    applicationBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        IExceptionHandlerFeature? ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                        {
                            string? errorMessage = JsonConvert.SerializeObject(new { error = ex.Error.Message });
                            await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
                        }
                    });
                })
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

        public static void PopulateDatabase(UserDbContext? context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

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
