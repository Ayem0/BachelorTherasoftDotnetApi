using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Base;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
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
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting {name} with id '{id}' : {ex.Message}", nameof(T), id, ex.Message);
            throw new DbException(DbAction.Read, nameof(T), id);
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
            _logger.LogError("Error creating {name} : {ex.Message}", nameof(T), ex.Message);
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
            _logger.LogError("Error updating {name} with id '{id}' : {ex.Message}", nameof(T), entity.Id, ex.Message);
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
            _logger.LogError("Error deleting {name} with id '{id}' : {ex.Message}", nameof(T), entity.Id, ex.Message);
            throw new DbException(DbAction.Delete, nameof(T), entity.Id);
        }
    }

    public async Task<bool> DeleteMultipleAsync(List<T> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                entity.DeletedAt = DateTime.UtcNow;
            }
            _dbSet.UpdateRange(entities);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting {name} with ids '{ids}' : {ex.Message}", nameof(T), entities.Select(e => e.Id), ex.Message);
            throw new DbException(DbAction.Delete, nameof(T));
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
            _logger.LogError("Error creating multiple {name} : {ex.Message}", nameof(T), ex.Message);
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
            _logger.LogError("Error updating multiple {name} with ids '{ids}' : {ex.Message}", nameof(T), entities.Select(e => e.Id), ex.Message);
            throw new DbException(DbAction.Update, nameof(T));
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
            _logger.LogError("Error getting {name} with ids '{ids}' : {ex.Message}", nameof(T), ids, ex.Message);
            throw new DbException(DbAction.Read, nameof(T));
        }
    }

}

