using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace WebCachedApplication.Middlewares
{
    public class ServerCacheMiddleware: IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cacheControlHeaders = context.Request.Headers.Where(HeaderIsCacheControlDisabled);
            foreach (var cacheControlHeader in cacheControlHeaders)
            {
                context.Request.Headers.Remove(cacheControlHeader);
            }
            var pragmaNoCacheHeaders = context.Request.Headers.Where(HeaderIsPragmaDisabled);
            foreach (var pragmaNoCacheHeader in pragmaNoCacheHeaders)
            {
                context.Request.Headers.Remove(pragmaNoCacheHeader);
            }
            return next(context);
        }

        private static bool HeaderIsCacheControlDisabled(KeyValuePair<string, StringValues> header)
        {
            var values = new[] { "no-cache", "no-store", "max-age=0" };
            return header.Key.Trim().ToLower() == "cache-control" && header.Value.Any(x => values.Contains(x.ToLower()));
        }

        private static bool HeaderIsPragmaDisabled(KeyValuePair<string, StringValues> x)
        {
            return x.Key.Trim().ToLower() == "pragma" && x.Value.Any(y => y.ToLower() == "no-cache");
        }

    }
}
