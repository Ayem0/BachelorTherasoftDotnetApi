using System.Text.Json;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Org.BouncyCastle.Crypto.Engines;
using StackExchange.Redis;

namespace BachelorTherasoftDotnetApi.src.Services;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _redisDb;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _redisDb = _connectionMultiplexer.GetDatabase();
    }

    public async Task SetHashAsync<T>(string key, T value, TimeSpan ttl) where T : class
    {
        var properties = typeof(T).GetProperties();
        var hashEntries = properties.Where(p => p.GetValue(value) != null).Select(p => new HashEntry(p.Name, p.GetValue(value)?.ToString())).ToArray();
        var setTask = _redisDb.HashSetAsync(key, hashEntries);
        var expireTask = _redisDb.KeyExpireAsync(key, ttl);
        await Task.WhenAll(setTask, expireTask);
    }

    public async Task SetHashesAsync<T>(IEnumerable<string> keys, IEnumerable<T> values, TimeSpan ttl) where T : class
    {
        var tasks = values.Zip(keys, (value, key) => SetHashAsync(key, value, ttl));
        await Task.WhenAll(tasks);
    }

    public async Task<T?> GetHashAsync<T>(string key) where T : class
    {
        var entries = await _redisDb.HashGetAllAsync(key);
        if (entries.Length > 0)
            return SerializeHasheEntries<T>(entries);
        return null;
    }

    public async Task<List<T>> GetHashesAsync<T>(params IEnumerable<string> keys) where T : class
    {
        var tasks = keys.Select(key => _redisDb.HashGetAllAsync(key));
        var results = await Task.WhenAll(tasks);
        var list = new List<T>(results.Length);
        foreach (var entries in results)
        {
            if (entries.Length == 0)
                continue;

            var value = SerializeHasheEntries<T>(entries);

            if (value != null)
                list.Add(value);
        }
        return list;
    }

    public async Task AddToSetAsync(string key, params IEnumerable<string> values)
    {
        var hasKey = await _redisDb.KeyExistsAsync(key);
        if (!hasKey) return;

        var redisValues = values.Select(v => new RedisValue(v)).ToArray();
        await _redisDb.SetAddAsync(key, redisValues);
    }

    public async Task AddSetAsync(string key, IEnumerable<string> values, TimeSpan ttl)
    {
        var hasKey = await _redisDb.KeyExistsAsync(key);
        if (hasKey) await DeleteKeyAsync(key);

        var redisValues = values.Select(v => new RedisValue(v)).ToArray();
        var setTask = _redisDb.SetAddAsync(key, redisValues);
        var expireTask = _redisDb.KeyExpireAsync(key, ttl);
        await Task.WhenAll(setTask, expireTask);
    }

    public async Task RemoveFromSetAsync(string key, string value)
    {
        await _redisDb.SetRemoveAsync(key, value);
    }

    public async Task DeleteKeyAsync(string key)
    {
        await _redisDb.KeyDeleteAsync(key);
    }

    public async Task<IEnumerable<string>> GetSetAsync(string key)
    {
        var set = await _redisDb.SetMembersAsync(key);
        return set.Select(v => v.ToString());
    }

    private static T? SerializeHasheEntries<T>(HashEntry[] entries) where T : class
    {
        var dictionnary = entries.ToDictionary(e =>
            e.Name.ToString(),
            e => e.Value.ToString()
        );
        var serialized = JsonSerializer.Serialize(dictionnary);
        T? value = JsonSerializer.Deserialize<T>(serialized);
        return value;
    }
}
