using StackExchange.Redis;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRedisService
{
    public Task DeleteAsync(string key);
    public Task<TEntity> GetOrSetAsync<TEntity>(
        string cacheKey,
        Func<Task<TEntity>> loader,
        TimeSpan ttl
    );
    public Task<TDto> GetOrSetAsync<TEntity, TDto>(
        string cacheKey,
        Func<Task<TEntity>> loader,
        TimeSpan ttl
    );
    public Task DeleteAsync(params RedisKey[] keys);
    public Task SetAsync<T>(string key, T value, TimeSpan ttl);
}
