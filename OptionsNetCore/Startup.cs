using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddlewareLifetimes.Middlewares;
using OptionsNetCore.Core.Options.Redis;
using OptionsNetCore.Services;

namespace OptionsNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHostedService<ListenerService>();

            //services.Configure<RedisOptions>(this.Configuration.GetSection(RedisOptions.Redis));

            services
                .AddOptions<RedisOptions>()
                .Bind(this.Configuration.GetSection(RedisOptions.Redis))
                .ValidateDataAnnotations()
                .Validate(config =>
                {
                    if(config.ConnectRetry > 3)
                        return config.ConnectTimeout > 5000;

                    return true;
                }, "ConnectTimeout must be > than 5000");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOptionsMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
