using System.Collections.Concurrent;

namespace BuildingBlocks.Cache;

public class InMemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, (object Value, DateTime Expiry)> _cache = new();

    public Task<bool> DeleteAsync(string key)
    {
        var removed = _cache.TryRemove(key, out _);
        return Task.FromResult(removed);
    }

    public Task<T?> GetAsync<T>(string key) where T : class
    {
        if (_cache.TryGetValue(key, out var cacheItem))
        {
            if (cacheItem.Expiry > DateTime.Now)
            {
                return Task.FromResult(cacheItem.Value as T);
            }

            _cache.TryRemove(key, out _);
        }

        return Task.FromResult<T?>(null);
    }

    public Task SaveAsync<T>(string key, T value, TimeSpan expiry) where T : class
    {
        var expiryDate = DateTime.Now.Add(expiry);
        _cache.AddOrUpdate(key, (value, expiryDate), (_, _) => (value, expiryDate));
        return Task.CompletedTask;
    }
}
