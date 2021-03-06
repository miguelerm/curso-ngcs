﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minfin.SSO.WebApi.Client;
using Minfin.SSO.WebApi.Client.Clients;

namespace Abs.Authentication.Service
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
            services.AddDataProtection()
                .SetApplicationName("ABS");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = "SSO_AUTH";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                });

            // SSO Dependencies:
            var ssoConfig = Configuration.GetSection("SSO");
            var privateKey = System.IO.File.ReadAllText(ssoConfig["PrivateKeyPath"]);
            var serviceEndpointUri = new Uri(ssoConfig["ServiceEndpoint"]);

            services.AddScoped(p => 
                new AuthenticationManager(privateKey)
            );

            services.AddScoped(p => 
                new TicketAutenticacionClient(serviceEndpointUri, p.GetRequiredService<AuthenticationManager>())
            );

            services.AddScoped(p => 
                new UsuarioClient(serviceEndpointUri, p.GetRequiredService<AuthenticationManager>())
            );
            services.AddScoped(p =>
                new AccesoClient(serviceEndpointUri, p.GetRequiredService<AuthenticationManager>())
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
