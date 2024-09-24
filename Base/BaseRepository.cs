using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Base;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly MySqlDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(MySqlDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbSet.Where(x => x.Id == id && x.DeletedAt == null).FirstAsync();
    }

    public async Task CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _dbSet.FindAsync(id);
        
        if (entity != null && entity.DeletedAt == null)
        {
            entity.DeletedAt = DateTime.Now;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

