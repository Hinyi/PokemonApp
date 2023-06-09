using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace PokemonApp.Service.CachingService
{
    public class CacheService : ICacheService
    {
        private static ConcurrentDictionary<string, bool> CacheKeys = new();
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key) 
            where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync(key);
            if (cachedValue is null)
            {
                return null;
            }

            T? value = JsonConvert.DeserializeObject<T>(cachedValue);

            return value;
        }

        public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory) 
            where T : class
        {
            T? cachedValue = await GetAsync<T>(key);
            if (cachedValue is not null)
            {
                return cachedValue;
            }

            cachedValue = await factory();

            await SetAsync(key, cachedValue);

            return cachedValue;
        }

        public async Task SetAsync<T>(string key, T value)
            where T : class
        {
            var expireTime = DateTimeOffset.Now.Subtract(DateTimeOffset.Now.AddSeconds(-30));
           
            string cachedValue = JsonConvert.SerializeObject(value);

            await _distributedCache.SetStringAsync(key, cachedValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expireTime
            });

            CacheKeys.TryAdd(key, true);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);

            CacheKeys.TryRemove(key, out bool _);
        }

        public async Task RemoveByPrefixAsync(string prefixKey)
        {
            IEnumerable<Task> tasks = CacheKeys
                .Keys
                .Where(k => k.StartsWith(prefixKey))
                .Select(k => RemoveAsync(k));

            await Task.WhenAll(tasks);
        }
    }
}
