using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebCachedApplication.Middlewares;
using WebCachedApplication.Models;
using WebCachedApplication.Repositories;

namespace WebCachedApplication
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
            services.AddMemoryCache();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IUsersRepository, UsersSqliteRepository>();
            services.Decorate<IUsersRepository, UsersCachedRepository>();
            services.AddSingleton<ServerCacheMiddleware>();
            services.AddHostedService<DatabaseInitializer>();
            services.AddResponseCaching();
            services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Users", new CacheProfile
                {
                    Duration = int.Parse(Configuration["AppSettings:DefaultResponseCacheExpiration"]),
                    VaryByQueryKeys = new [] { "page" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ServerCacheMiddleware>();
            app.UseResponseCaching();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
