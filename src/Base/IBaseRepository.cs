using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Base;

public interface IBaseRepository<T> where T : class, IBaseEntity
{
    Task<T?> GetByIdAsync(string id);
    Task<List<T>> GetByIdsAsync(List<string> ids);
    Task<T> CreateAsync(T entity);
    Task<List<T>> CreateMultipleAsync(List<T> entities);
    Task<T> UpdateAsync(T entity);
    Task<List<T>> UpdateMultipleAsync(List<T> entities);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteMultipleAsync(List<T> entities);
}

