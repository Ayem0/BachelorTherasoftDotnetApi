using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class AreaRepository : BaseRepository<Area>, IAreaRepository
{
    public AreaRepository(MySqlDbContext context, ILogger<Area> logger) : base(context, logger)
    {
    }

    public async Task<Area?> GetByIdJoinRoomsAsync(string id)
    {
        return await _dbSet
            .Include(w => w.Rooms)
            .Where(w => w.Id == id && w.DeletedAt == null && w.Location.DeletedAt == null && w.Location.Workspace.DeletedAt == null
                && w.Rooms.All(x => x.DeletedAt == null))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Area>> GetAreasByLocationIdAsync(string id)
    {
        return await _dbSet
            .Where(a => a.LocationId == id && a.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<Area?> GetByIdJoinWorkspaceAsync(string id)
    {
        return await _dbSet
            .Include(w => w.Workspace)
            .Where(w => w.Id == id && w.DeletedAt == null && w.Location.DeletedAt == null && w.Workspace.DeletedAt == null)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Area>> GetAreasByWorkspaceIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Where(x => x.WorkspaceId == id)
                .ToListAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace for user with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Area by workspace", id);
        }
    }
}
