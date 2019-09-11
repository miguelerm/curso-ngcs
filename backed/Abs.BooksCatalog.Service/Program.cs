using Abs.BooksCatalog.Service.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Abs.BooksCatalog.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
