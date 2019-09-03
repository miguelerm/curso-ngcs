using Abs.FilesManager.Services.Consumers;
using Abs.FilesManager.Services.Observers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Abs.FilesManager.Services
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IConsumeObserver, LoggingObserver>();

            services.AddMassTransit(x => {

                x.AddConsumer<PutFilesConsumer>();
                x.AddConsumer<BookCreatedConsumer>();
                x.AddConsumer<FileCreatedConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    var host = config.Host("localhost", "demos", h => {
                        h.Username("demo-user");
                        h.Password("demo-user");
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
