using Microsoft.Extensions.Caching.Memory;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _defaultExpiry = TimeSpan.FromMinutes(10);

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? Get<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? _defaultExpiry
            };
            _cache.Set(key, value, options);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
