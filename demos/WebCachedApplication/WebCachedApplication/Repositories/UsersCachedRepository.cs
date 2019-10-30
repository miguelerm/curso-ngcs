using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using WebCachedApplication.Entities;
using WebCachedApplication.Models;

namespace WebCachedApplication.Repositories
{
    public class UsersCachedRepository: IUsersRepository
    {
        private readonly IMemoryCache cache;
        private readonly IUsersRepository actualRepository;
        private readonly ILogger<UsersCachedRepository> logger;
        private readonly AppSettings settings;

        public UsersCachedRepository(IMemoryCache cache, IOptions<AppSettings> options, IUsersRepository actualRepository, ILogger<UsersCachedRepository> logger = null)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.actualRepository = actualRepository ?? throw new ArgumentNullException(nameof(actualRepository));
            this.logger = logger ?? NullLogger<UsersCachedRepository>.Instance;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            const string cacheKey = "Users";

            logger.LogDebug("Looking up {key} on cache", cacheKey);
            return cache.GetOrCreateAsync(cacheKey, entry =>
            {
                logger.LogDebug("No cached entry available for {key}.", entry.Key);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(settings.DefaultInMemoryCacheExpiration);
                return actualRepository.GetAllAsync();
            });
        }
    }
}
