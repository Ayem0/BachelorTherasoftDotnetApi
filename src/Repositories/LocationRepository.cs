using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;


namespace BachelorTherasoftDotnetApi.src.Repositories;

public class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    public LocationRepository(MySqlDbContext context, ILogger<Location> logger) : base(context, logger)
    {
    }

    public async Task<Location?> GetByIdJoinWorkspaceAsync(string id)
    {
        try
        {
            return await _dbSet
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
            return await _dbSet
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
            return await _dbSet
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
}
