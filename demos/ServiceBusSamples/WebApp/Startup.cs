using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IHostedService = Microsoft.Extensions.Hosting.IHostedService;
using Microsoft.Extensions.Logging;
using WebApp.Consumers;
using WebApp.Hosts;
using MassTransit.Audit;
using WebApp.Stores;

namespace WebApp
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

            services.AddSingleton<IMessageAuditStore, MessageAuditStore>();
            services.AddMassTransit(x => {

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    var host = config.Host("localhost", "demos", h => {
                        h.Username("demo-user");
                        h.Password("demo-user");
                    });

                    config.ReceiveEndpoint(host, "web-app", endpoint =>
                    {
                        endpoint.Consumer<SayHelloConsumer>();
                    });

                    config.UseExtensionsLogging(provider.GetRequiredService<ILoggerFactory>());
                }));

            });

            services.AddSingleton<IHostedService, BusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
