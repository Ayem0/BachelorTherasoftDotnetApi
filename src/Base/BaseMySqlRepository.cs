using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
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

    public async Task<T?> GetEntityByIdAsync(string id)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting {nameof(T)} with ID '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, nameof(T), id);
        }
    }
    
    // public async Task<TDto?> GetByIdAsync<TDto>(string id)
    // {
    //     IQueryable<T> query = _dbSet;

    //     var navigations = _context.Model.FindEntityType(typeof(T))?.GetNavigations();
        
    //     if (navigations != null)
    //     {
    //         var properties = typeof(TDto).GetProperties().Select(p => p.Name).ToHashSet();
            
    //         foreach (var property in properties)
    //         {
    //             if (navigations.Select(x => x.Name).Contains(property))
    //             {                 
    //                 query = query.Include(property);
    //             }
    //         }
    //     }

    //     T? entity = await query.Where(x => x.Id == id && x.DeletedAt == null).FirstOrDefaultAsync();

    //     return entity == null ? default :_mapper.Map<TDto>(entity);
    // }

    public async Task CreateAsync(T entity)
    {
        try
        {
            entity.CreatedAt = DateTime.UtcNow;
            _dbSet.Add(entity);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating {nameof(T)} : {ex.Message}");
            throw new DbException(DbAction.Create, nameof(T));
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating {nameof(entity)} with ID '{entity.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, nameof(entity), entity.Id);
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var res = await _dbSet.Where(x => x.Id == id && x.DeletedAt == null).ExecuteUpdateAsync(x => x.SetProperty(x => x.DeletedAt, DateTime.UtcNow));
            return res > 0;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting {nameof(T)} with ID '{id}' : {ex.Message}");
            throw new DbException(DbAction.Delete, nameof(T), id);
        }
    }

    public async Task CreateMultipleAsync(List<T> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            _dbSet.AddRange(entities);

            await _context.SaveChangesAsync();

           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating mutliple {nameof(T)} : {ex.Message}");
            throw new DbException(DbAction.Create, nameof(T));
        }
    }
}

