using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Base;

public interface IBaseRepository<T> where T : BaseModel
{
    Task<DbResponse<T>> GetEntityByIdAsync(string id);
    Task<TDto?> GetByIdAsync<TDto>(string id);
    Task<DbResponse<T>> CreateAsync(T entity);
    Task<DbResponse<T>> UpdateAsync(T entity);
    Task<DbResponse<T>> DeleteAsync(string id);
    Task<DbResponse<T>> CreateMultipleAsync(List<T> entities);
}

