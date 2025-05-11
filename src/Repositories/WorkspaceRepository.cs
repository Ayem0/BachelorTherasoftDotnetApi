using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepository(MySqlDbContext context, ILogger<Workspace> logger) : base(context, logger)
    {
    }

    public async Task<Workspace[]> GetByUserIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Where(x => x.Users.Any(x => x.Id == id && x.DeletedAt == null) && x.DeletedAt == null)
                .ToArrayAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace for user with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }
    }

    public async Task<Workspace?> GetDetailsByIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Include(x => x.Users.Where(y => y.DeletedAt == null))
                .Include(x => x.Locations.Where(y => y.DeletedAt == null))
                    .ThenInclude(x => x.Areas.Where(y => y.DeletedAt == null))
                        .ThenInclude(x => x.Rooms.Where(y => y.DeletedAt == null))
                .Include(x => x.WorkspaceRoles.Where(y => y.DeletedAt == null))
                .Include(x => x.Tags.Where(y => y.DeletedAt == null))
                .Include(x => x.EventCategories.Where(y => y.DeletedAt == null))
                .Include(x => x.Slots.Where(y => y.DeletedAt == null))
                .Include(x => x.Participants.Where(y => y.DeletedAt == null))
                .Include(x => x.ParticipantCategories.Where(y => y.DeletedAt == null))
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }
    }

    public async Task<Workspace?> GetJoinUsersByIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Include(x => x.Users.Where(y => y.DeletedAt == null))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workspace with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Workspace", id);
        }
    }
}
