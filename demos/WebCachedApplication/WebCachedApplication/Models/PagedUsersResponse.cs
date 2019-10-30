using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCachedApplication.Entities;

namespace WebCachedApplication.Models
{
    public class PagedUsersResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<User> Items { get; set; }
        public DateTime ResponseDate { get; set; } = DateTime.UtcNow;
    }
}
