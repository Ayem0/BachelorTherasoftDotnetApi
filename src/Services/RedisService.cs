using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using StackExchange.Redis;

namespace BachelorTherasoftDotnetApi.src.Services;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _redisDb;
    private readonly IMapper _mapper;

    public RedisService(IConnectionMultiplexer connectionMultiplexer, IMapper mapper)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _redisDb = _connectionMultiplexer.GetDatabase();
        _mapper = mapper;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var serialized = JsonSerializer.Serialize(value, options: new() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
        await _redisDb.StringSetAsync(key, serialized, ttl);
    }

    private async Task<T?> GetAsync<T>(string key)
    {
        var entries = await _redisDb.StringGetAsync(key);
        var json = entries.ToString();
        if (string.IsNullOrEmpty(json)) 
            return default;
        return JsonSerializer.Deserialize<T?>(json, options: new() { ReferenceHandler = ReferenceHandler.IgnoreCycles });
    }

    public async Task DeleteAsync(string key)
    {
        await _redisDb.KeyDeleteAsync(key);
    }

    public async Task DeleteAsync(params RedisKey[] keys)
    {
        await _redisDb.KeyDeleteAsync(keys);
    }

    private static T? DeserializeHasheEntries<T>(HashEntry[] entries)
    {
        var dictionnary = entries.ToDictionary(e =>
            e.Name.ToString(),
            e => e.Value.ToString()
        );
        var serialized = JsonSerializer.Serialize(dictionnary);
        T? value = JsonSerializer.Deserialize<T>(serialized);
        return value;
    }

    public async Task<TEntity> GetOrSetAsync<TEntity>(string cacheKey, Func<Task<TEntity>> loader, TimeSpan ttl)
    {
        var cached = await GetAsync<TEntity>(cacheKey);
        if (cached != null)
            return cached;
        var fresh = await loader();
        if (fresh != null)
            await SetAsync(cacheKey, fresh, ttl);
        return fresh;
    }

    public async Task<TDto> GetOrSetAsync<TEntity, TDto>(string cacheKey, Func<Task<TEntity>> loader, TimeSpan ttl)
    {
        var cached = await GetAsync<TEntity>(cacheKey);
        if (cached != null)
            return _mapper.Map<TDto>(cached);
        var fresh = await loader();
        if (fresh != null)
            await SetAsync(cacheKey, fresh, ttl);
        return _mapper.Map<TDto>(fresh);
    }





    // public async Task SetHashesAsync<T>(IEnumerable<string> keys, IEnumerable<T> values, TimeSpan ttl) where T : class
    // {
    //     var tasks = values.Zip(keys, (value, key) => SetHashAsync(key, value, ttl));
    //     await Task.WhenAll(tasks);
    // }



    // public async Task<List<T>> GetHashesAsync<T>(params IEnumerable<string> keys) where T : class
    // {
    //     var tasks = keys.Select(key => _redisDb.HashGetAllAsync(key));
    //     var results = await Task.WhenAll(tasks);
    //     var list = new List<T>(results.Length);
    //     foreach (var entries in results)
    //     {
    //         if (entries.Length == 0)
    //             continue;

    //         var value = DeserializeHasheEntries<T>(entries);

    //         if (value != null)
    //             list.Add(value);
    //     }
    //     return list;
    // }

    // public async Task AddToSetAsync(string key, params IEnumerable<string> values)
    // {
    //     var hasKey = await _redisDb.KeyExistsAsync(key);
    //     if (!hasKey) return;

    //     var redisValues = values.Select(v => new RedisValue(v)).ToArray();
    //     await _redisDb.SetAddAsync(key, redisValues);
    // }

    // public async Task AddSetAsync(string key, IEnumerable<string> values, TimeSpan ttl)
    // {
    //     var hasKey = await _redisDb.KeyExistsAsync(key);
    //     if (hasKey) await DeleteKeyAsync(key);

    //     var redisValues = values.Select(v => new RedisValue(v)).ToArray();
    //     var setTask = _redisDb.SetAddAsync(key, redisValues);
    //     var expireTask = _redisDb.KeyExpireAsync(key, ttl);
    //     await Task.WhenAll(setTask, expireTask);
    // }

    // public async Task RemoveFromSetAsync(string key, string value)
    // {
    //     await _redisDb.SetRemoveAsync(key, value);
    // }


    // public async Task<IEnumerable<string>> GetSetAsync(string key)
    // {
    //     var set = await _redisDb.SetMembersAsync(key);
    //     return set.Select(v => v.ToString());
    // }
}
