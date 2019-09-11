using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abs.BooksCatalog.Service.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Abs.BooksCatalog.Service
{
    public class MigrationsService: IHostedService
    {
        private readonly IServiceProvider services;
        private readonly ILogger<MigrationsService> logger;

        public MigrationsService(IServiceProvider services, ILogger<MigrationsService> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug("Trying to run migrations");
            using (var scope = services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<BooksCatalogContext>();
                try
                {
                    await db.Database.MigrateAsync(cancellationToken);
                    logger.LogDebug("Migrations executed.");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Database Creation/Migrations failed!");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
