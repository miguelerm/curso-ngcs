using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abs.BooksCatalog.Service.Clients;
using Abs.BooksCatalog.Service.Consumers;
using Abs.BooksCatalog.Service.Data;
using Abs.FilesManager.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Abs.BooksCatalog.Service
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
            services.AddDbContext<BooksCatalogContext>(options => {
                //options.UseInMemoryDatabase("BooksDb");
                options.UseSqlite("Data Source=books.db");

            });

            services.AddHttpClient();
            services.AddTransient<FilesClient>();
            services.AddHttpClient<FilesClient>();

            services.AddMassTransit(x => {

                x.AddConsumer<GetBookByIdConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    var host = config.Host("localhost", "demos", h => {
                        h.Username("demo-user");
                        h.Password("demo-user");
                    });

                    config.ReceiveEndpoint(host, "books-manager", endpoint =>
                    {
                        endpoint.ConfigureConsumer<GetBookByIdConsumer>(provider);
                    });
                }));

            });

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider sp)
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
        }
    }
}
