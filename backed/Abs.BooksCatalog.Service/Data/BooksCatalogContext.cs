using Microsoft.EntityFrameworkCore;

namespace Abs.BooksCatalog.Service.Data
{
    public class BooksCatalogContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public BooksCatalogContext(DbContextOptions<BooksCatalogContext> options) : base(options)
        {
            
        }
    }
}