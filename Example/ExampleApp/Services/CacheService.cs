using Microsoft.Extensions.Caching.Memory;

namespace ExampleApp.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Add(string key, string value)
        {
            _cache.Set(key, value);
        }

        public bool Has(string key)
        {
            return _cache.TryGetValue(key, out string _);
        }
    }
}
