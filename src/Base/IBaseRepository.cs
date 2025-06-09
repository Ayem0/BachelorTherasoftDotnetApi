using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Base;

public interface IBaseRepository<T> where T : class, IBaseEntity
{
    Task<T?> GetByIdAsync(string id);
    Task<List<T>> GetByIdsAsync(List<string> ids);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<List<T>> CreateMultipleAsync(List<T> entities);
    Task<List<T>> UpdateMultipleAsync(List<T> entities);
}

