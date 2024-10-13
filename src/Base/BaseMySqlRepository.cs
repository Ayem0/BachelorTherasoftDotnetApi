using AutoMapper;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseMySqlRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly MySqlDbContext _context;
    private readonly DbSet<T> _dbSet;
    protected readonly IMapper _mapper;

    public BaseMySqlRepository(MySqlDbContext context, IMapper mapper)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _mapper = mapper;
    }

    public async Task<DbResponse<T>> GetEntityByIdAsync(string id)
    {
        try
        {
            var res = await _dbSet.Where(x => x.Id == id && x.DeletedAt == null).FirstOrDefaultAsync();

            return new(res);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting {nameof(T)} with ID '{id}' : {ex.Message}");
            return new($"Error getting {nameof(T)} with ID '{id}'.", ex.Message);
        }
    }
    
    public async Task<TDto?> GetByIdAsync<TDto>(string id)
    {
        IQueryable<T> query = _dbSet;

        var navigations = _context.Model.FindEntityType(typeof(T))?.GetNavigations();
        
        if (navigations != null)
        {
            var properties = typeof(TDto).GetProperties().Select(p => p.Name).ToHashSet();
            
            foreach (var property in properties)
            {
                if (navigations.Select(x => x.Name).Contains(property))
                {                    
                    query = query.Include(property);
                }
            }
        }

        T? entity = await query.Where(x => x.Id == id && x.DeletedAt == null).FirstOrDefaultAsync();

        return entity == null ? default :_mapper.Map<TDto>(entity);
    }

    public async Task<DbResponse<T>> CreateAsync(T entity)
    {
        try
        {
            entity.CreatedAt = DateTime.Now;
            _dbSet.Add(entity);
            var res = await _context.SaveChangesAsync();

            if (res > 0) return new();

            return new($"Error creating {nameof(T)}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating {nameof(T)} : {ex.Message}");
            return new($"Error creating {nameof(T)}.", ex.Message);
        }
    }

    public async Task<DbResponse<T>> UpdateAsync(T entity)
    {
        try
        {
            entity.UpdatedAt = DateTime.Now;
            _dbSet.Update(entity);
            var res = await _context.SaveChangesAsync();

            if (res > 0) return new();

            return new($"Error updating {nameof(T)} with ID '{entity.Id}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating {nameof(T)} with ID '{entity.Id}' : {ex.Message}");
            return new($"Error updating {nameof(T)} with ID '{entity.Id}'.", ex.Message);
        }
    }

    public async Task<DbResponse<T>> DeleteAsync(string id)
    {
        try
        {
            var res = await _dbSet.Where(x => x.Id == id && x.DeletedAt == null).ExecuteUpdateAsync(x => x.SetProperty(x => x.DeletedAt, DateTime.Now));

            if (res > 0) return new();

            return new($"{nameof(T)} with ID '{id}' already deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting {nameof(T)} with ID '{id}' : {ex.Message}");
            return new($"Error deleting {nameof(T)} with ID '{id}'.", ex.Message);
        }
    }

    public async Task<DbResponse<T>> CreateMultipleAsync(List<T> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.Now;
            }
            _dbSet.AddRange(entities);

            var res = await _context.SaveChangesAsync();
            
            if (res > 0) return new();

            return new($"Error creating mutliple {nameof(T)}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating mutliple {nameof(T)} : {ex.Message}");
            return new($"Error creating mutliple {nameof(T)}.", ex.Message);
        }
    }
}

