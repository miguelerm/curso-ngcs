using Abs.FilesManager.Services.Consumers;
using Abs.FilesManager.Services.Observers;
using Abs.Messages.BooksCatalog.Queries;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Abs.FilesManager.Services
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<FilesConfig>(configuration.GetSection("Files"));

            services.AddTransient<IConsumeObserver, LoggingObserver>();

            var hostConfig = configuration.GetSection("Bus");
            var hostUrl = hostConfig["Host"];

            services.AddMassTransit(x => {

                x.AddConsumer<PutFilesConsumer>();
                x.AddConsumer<BookCreatedConsumer>();
                x.AddConsumer<FileCreatedConsumer>();
                x.AddRequestClient<IGetBookByIdRequest>(new Uri(hostUrl + "/books-manager"));

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    var host = config.Host(new Uri(hostUrl), h =>
                    {
                        h.Username(hostConfig["Username"]);
                        h.Password(hostConfig["Password"]);
                    });

                    config.ReceiveEndpoint(host, "files-service", endpoint =>
                    {
                        endpoint.ConfigureConsumer<PutFilesConsumer>(provider);
                        endpoint.ConfigureConsumer<BookCreatedConsumer>(provider);
                        endpoint.ConfigureConsumer<FileCreatedConsumer>(provider);
                    });

                    
                    config.UseSerilog();
                    config.UseExtensionsLogging(provider.GetRequiredService<ILoggerFactory>());

                }));

            });

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            logger.LogTrace("Test log Trace");
            logger.LogDebug("Test log Debug");
            logger.LogInformation("Test log Information");
            logger.LogWarning("Test log Warning");
            logger.LogError("Test log Error");
            logger.LogCritical("Test log Critical");
        }
    }
}
