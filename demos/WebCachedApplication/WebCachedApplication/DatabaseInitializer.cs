using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WebCachedApplication.Entities;
using WebCachedApplication.Properties;

namespace WebCachedApplication
{
    public class DatabaseInitializer : IHostedService
    {
        private readonly IConfiguration config;
        private readonly ILogger<DatabaseInitializer> logger;

        public DatabaseInitializer(IConfiguration config, ILogger<DatabaseInitializer> logger = null)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.logger = logger ?? NullLogger<DatabaseInitializer>.Instance;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (File.Exists("Database.db"))
            {
                return;
            }

            var connectionString = config.GetConnectionString("Default");
            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            await connection.ExecuteAsync(SqlScripts.UsersCreate);

            var userIds = 0;
            var testUsers = new Faker<User>()
                //Optional: Call for objects that have complex initialization
                .CustomInstantiator(f => new User { Id = userIds++, SSN = f.Random.Replace("###-##-####") })

                //Use an enum outside scope.
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())

                //Basic rules using built-in generators
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender == Gender.Female ? Name.Gender.Female : Name.Gender.Male))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender == Gender.Female ? Name.Gender.Female : Name.Gender.Male))
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")

                //Use a method outside scope.
                .RuleFor(u => u.CartId, f => Guid.NewGuid().ToString())
                //Compound property with context, use the first/last name properties
                .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
                //Optional: After all rules are applied finish with the following action
                .FinishWith((f, u) =>
                {
                    logger.LogDebug("User Created! Id={id}", u.Id);
                });

            var users = testUsers.Generate(500);
            await connection.ExecuteAsync(SqlScripts.UsersInsert, users);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
