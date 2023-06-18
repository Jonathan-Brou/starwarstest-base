
using Domain.Interfaces;
using DataAccess.Clients;
using Services;
using starwarstest_api.middleware;

namespace starwarstest_api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }
        private const string CorsAppOrigin = "CORS_AppOrigin";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(CorsAppOrigin,
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });

            services.AddMemoryCache();
            services.AddScoped<IStarWarsService, StarWarsService>();
            services.AddScoped<ISwapapiClient, SwapapiClient>();
            services.AddHttpClient();
            services.AddHostedService<LoadingService>();
            services.AddSingleton<IInitializationService, InitializationService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<InitializationMiddleware>();


            app.UseCors(CorsAppOrigin);

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}