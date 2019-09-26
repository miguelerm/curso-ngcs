using Abs.BooksCatalog.Service.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Abs.BooksCatalog.Service.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly IConfiguration config;

        public BooksRepository(IConfiguration config)
        {
            this.config = config;
        }
        public async Task<IEnumerable<Book>> GetAllAsync(string criteria)
        {
            criteria = $"%{criteria?.ToLower() ?? ""}%";

            var connectionString = config.GetConnectionString("Default");
            using (var cnn = new SqliteConnection(connectionString))
            {
                await cnn.OpenAsync();
                var result = await cnn.QueryAsync<Book>("SELECT * FROM Books WHERE lower(Title) LIKE @criteria", new { criteria });
                return result.ToArray();
            }
        }
    }
}
