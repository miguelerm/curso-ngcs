namespace WebCachedApplication.Models
{
    public class AppSettings
    {
        public int DefaultPageSize { get; set; }
        public int DefaultResponseCacheExpiration { get; set; }
        public int DefaultInMemoryCacheExpiration { get; set; }
    }
}
