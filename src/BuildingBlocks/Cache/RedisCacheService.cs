using Newtonsoft.Json;
using StackExchange.Redis;

namespace BuildingBlocks.Cache;

public interface ICacheService
{
    Task<bool> DeleteAsync(string key);
    Task<T?> GetAsync<T>(string key) where T : class;
    Task SaveAsync<T>(string key, T value, TimeSpan expiry) where T : class;
}

public class RedisCacheService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteAsync(string key) => await _database.KeyDeleteAsync(key);

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var data = await _database.StringGetAsync(key);

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        var result = data.ToString();

        return JsonConvert.DeserializeObject<T>(result);
    }

    public async Task SaveAsync<T>(string key, T value, TimeSpan expiry) where T : class
    {
        var data = JsonConvert.SerializeObject(value);

        await _database.StringSetAsync(key, data, expiry);
    }
}
