namespace BachelorTherasoftDotnetApi.src.Base;

public interface IBaseRepository<T> where T : BaseModel
{
    Task<T?> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task CreateMultipleAsync(List<T> entities);
}

