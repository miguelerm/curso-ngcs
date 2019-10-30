using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WebCachedApplication.Entities;
using WebCachedApplication.Properties;

namespace WebCachedApplication.Repositories
{
    public class UsersSqliteRepository: IUsersRepository
    {
        private readonly ILogger<UsersSqliteRepository> logger;
        private readonly string connectionString;

        public UsersSqliteRepository(IConfiguration configuration, ILogger<UsersSqliteRepository> logger = null)
        {
            connectionString = configuration.GetConnectionString("Default");
            this.logger = logger ?? NullLogger<UsersSqliteRepository>.Instance;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            logger.LogDebug("Getting users from database");
            await using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            var result = await connection.QueryAsync<User>(SqlScripts.UsersGetAll);
            return result.ToArray();
        }
    }
}
