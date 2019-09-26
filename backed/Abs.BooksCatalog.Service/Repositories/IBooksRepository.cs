using System.Collections.Generic;
using System.Threading.Tasks;
using Abs.BooksCatalog.Service.Data;

namespace Abs.BooksCatalog.Service.Repositories
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> GetAllAsync(string criteria);
    }
}