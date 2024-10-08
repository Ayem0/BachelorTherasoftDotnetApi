using BachelorTherasoftDotnetApi.src.Databases;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseMySqlRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly MySqlDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseMySqlRepository(MySqlDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        IQueryable<T> query = _dbSet;
        var navigations = _context.Model.FindEntityType(typeof(T))!.GetNavigations();
        foreach (var navigation in navigations)
        {
            query = query.Include(navigation.Name);
        }
        return await query.Where(x => x.Id == id && x.DeletedAt == null).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.Now;
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        entity.DeletedAt = DateTime.Now;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task CreateMultipleAsync(List<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreatedAt = DateTime.Now;
        }
        _dbSet.AddRange(entities);
        await _context.SaveChangesAsync();
    }
}

