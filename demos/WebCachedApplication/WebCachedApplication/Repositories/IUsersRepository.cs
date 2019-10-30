using System.Collections.Generic;
using System.Threading.Tasks;
using WebCachedApplication.Entities;

namespace WebCachedApplication.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
    }
}
