using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using WebCachedApplication.Entities;
using WebCachedApplication.Models;
using WebCachedApplication.Repositories;

namespace WebCachedApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository usersRepository;
        private readonly ILogger<UsersController> logger;
        private readonly AppSettings settings;

        public UsersController(IUsersRepository usersRepository, IOptions<AppSettings> options, ILogger<UsersController> logger = null)
        {
            settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.logger = logger ?? NullLogger<UsersController>.Instance;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Users")]
        public async Task<PagedUsersResponse> GetAsync(int page = 1)
        {
            var pageSize = settings.DefaultPageSize;
            var skip = (page - 1) * pageSize;
            var result = await usersRepository.GetAllAsync();
            var allUsers = result as User[] ?? result.ToArray();
            
            return new PagedUsersResponse
            {
                Count = allUsers.Length,
                Page = page,
                PageSize = pageSize,
                Items = allUsers.Skip(skip).Take(pageSize)
            };
        }
    }
}
