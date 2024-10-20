using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Base;

public interface IBaseRepository<T> where T : BaseModel
{
    Task<T?> GetEntityByIdAsync(string id);
    // Task<TDto?> GetByIdAsync<TDto>(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(string id);
    Task CreateMultipleAsync(List<T> entities);
}

