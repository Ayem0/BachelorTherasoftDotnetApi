using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;


namespace BachelorTherasoftDotnetApi.src.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly MySqlDbContext _context;
    public LocationRepository(MySqlDbContext context) 
    {
        _context = context;
    }

    public async Task<Location> CreateAsync(Location location)
    {
        try
        {    
            _context.Location.Add(location);
            var res = await _context.SaveChangesAsync();
            return location ?? throw new DbException(DbAction.Create, "Location");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Location : {ex.Message}");
            throw new DbException(DbAction.Create, "Location");
        }   
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {    
            var res = await _context.Location
                .Where(x => x.Id == id && x.DeletedAt == null)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.DeletedAt, DateTime.UtcNow));
            
            return res > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting location with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Delete, "Location", id);
        }   
    }

    public async Task<Location?> GetByIdAsync(string id)
    {
        try
        {    
            return await _context.Location
                .Where(x => x.Id == id && x.DeletedAt == null && x.Workspace.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting location with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Location", id);
        }   
    }

    public async Task<Location?> GetByIdJoinWorkspaceAsync(string id)
    {
        try
        {    
            return await _context.Location
                .Include(x => x.Workspace)
                .Where(x => x.Id == id && x.DeletedAt == null && x.Workspace.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting location with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Location", id);
        }   
    }

    public async Task<List<Location>> GetByWorkspaceIdAsync(string id)
    {
        try
        {    
            return await _context.Location
                .Where(x => x.WorkspaceId == id && x.DeletedAt == null)
                .ToListAsync();
                
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting location for user with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Location", id);
        }   
    }

    public async Task<Location?> GetDetailsByIdAsync(string id)
    {
        try
        {    
            return await _context.Location
                .Include(x => x.Areas.Where(y => y.DeletedAt == null))
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting location with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Location", id);
        }   
    }

    public async Task<Location> UpdateAsync(Location location)
    {
        try
        {    
            location.UpdatedAt = DateTime.UtcNow;
            var res = await _context.SaveChangesAsync();
            return res > 0 ? location : throw new DbException(DbAction.Update, "Location", location.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating location with Id '{location.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, "Location", location.Id);
        }   
    }
}
