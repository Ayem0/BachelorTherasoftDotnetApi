using BachelorTherasoftDotnetApi.src.Databases;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseMongoDbRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly MongoDbContext _context;
    private readonly DbSet<T> _dbSet;
    public BaseMongoDbRepository(MongoDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public async Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.Now;
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        entity.DeletedAt = DateTime.Now;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        IQueryable<T> query = _dbSet;

        // Charger toutes les relations (propriétés de navigation)
        var navigations = _context.Model.FindEntityType(typeof(T))!.GetNavigations();

        foreach (var navigation in navigations)
        {
            query = query.Include(navigation.Name);
        }

        return await query.Where(x => x.Id == id && x.DeletedAt == null).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}
