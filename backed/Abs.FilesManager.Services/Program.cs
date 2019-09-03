using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Abs.FilesManager.Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Abs.FilesManager.Services.Consumers", LogEventLevel.Verbose)
                .WriteTo.ColoredConsole()
                .CreateLogger();

            CreateWebHostBuilder(args)
                .ConfigureLogging(c =>
                {
                    c.ClearProviders();
                    c.AddSerilog(logger);
                })
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
