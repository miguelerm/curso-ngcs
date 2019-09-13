using System;
using System.Collections;
using Abs.BooksCatalog.Service.Clients;
using Abs.BooksCatalog.Service.Consumers;
using Abs.BooksCatalog.Service.Data;
using Abs.FilesManager.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Abs.BooksCatalog.Service
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<BooksCatalogContext>(options => {
                //options.UseInMemoryDatabase("BooksDb");
                options.UseSqlite(configuration.GetConnectionString("Default"));
            });

            services.AddHttpClient();
            services.AddTransient<FilesClient>();
            services.AddHttpClient<FilesClient>();

            services.AddMassTransit(x =>
            {

                x.AddConsumer<GetBookByIdConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    var hostConfig = configuration.GetSection("Bus");
                    var host = config.Host(new Uri(hostConfig["Host"]), h =>
                    {
                        h.Username(hostConfig["Username"]);
                        h.Password(hostConfig["Password"]);
                    });

                    config.ReceiveEndpoint(host, "books-manager", endpoint =>
                    {
                        endpoint.ConfigureConsumer<GetBookByIdConsumer>(provider);
                    });
                }));

            });

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusService>();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, MigrationsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                PrintEnvironmentVariables();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void PrintEnvironmentVariables()
        {
            if (!logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

            var envrionment = string.Empty;
            foreach (DictionaryEntry item in Environment.GetEnvironmentVariables())
            {
                envrionment += $"{item.Key}={item.Value}{Environment.NewLine}";
            }

            logger.LogTrace("Environment variables: {vars}", envrionment);
        }
    }
}
