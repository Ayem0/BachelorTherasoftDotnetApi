using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
{
    protected readonly MySqlDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ILogger<T> _logger;

    public BaseRepository(MySqlDbContext context, ILogger<T> logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _logger = logger;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting {nameof(T)} with ID '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, nameof(T), id);
        }
    }

    public async Task<List<T>> GetByIdsAsync(List<string> ids)
    {
        try
        {
            return await _dbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting {nameof(T)} with ID '{ids}' : {ex.Message}");
            throw new DbException(DbAction.Read, nameof(T), "");
        }
    }

    public async Task<T> CreateAsync(T entity)
    {
        try
        {
            entity.Id = Guid.NewGuid().ToString();
            _dbSet.Add(entity);

            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating {nameof(T)} : {ex.Message}");
            throw new DbException(DbAction.Create, nameof(T));
        }
    }

    public async Task<T> UpdateAsync(T entity)
    {
        try
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating {nameof(entity)} with ID '{entity.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, nameof(entity), entity.Id);
        }
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            entity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting {nameof(T)} with ID '{entity.Id}' : {ex.Message}");
            throw new DbException(DbAction.Delete, nameof(T), entity.Id);
        }
    }

    public async Task<List<T>> CreateMultipleAsync(List<T> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                entity.Id = Guid.NewGuid().ToString();
            }
            _dbSet.AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating mutliple {nameof(T)} : {ex.Message}");
            throw new DbException(DbAction.Create, nameof(T));
        }
    }

    public async Task<List<T>> UpdateMultipleAsync(List<T> entities)
    {
        try
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating mutliple {nameof(T)} : {ex.Message}");
            throw new DbException(DbAction.Update, nameof(T));
        }
    }
}

