using System;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRedisService
{
    public Task SetHashAsync<T>(string key, T value, TimeSpan ttl) where T : class;
    public Task SetHashesAsync<T>(IEnumerable<string> keys, IEnumerable<T> values, TimeSpan ttl) where T : class;
    public Task<T?> GetHashAsync<T>(string key) where T : class;
    public Task<List<T>> GetHashesAsync<T>(params IEnumerable<string> keys) where T : class;
    public Task AddSetAsync(string key, IEnumerable<string> values, TimeSpan ttl);
    public Task<IEnumerable<string>> GetSetAsync(string key);
    public Task AddToSetAsync(string key, params IEnumerable<string> values);
    public Task RemoveFromSetAsync(string key, string value);
    public Task DeleteKeyAsync(string key);
}
