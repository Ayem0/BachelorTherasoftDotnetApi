using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(MySqlDbContext context, ILogger<Room> logger) : base(context, logger)
    {
    }

    public async Task<List<Room>?> GetByAreaIdAsync(string id)
    {
        try
        {
            return await _context.Room
                .Where(x => x.AreaId == id && x.DeletedAt == null && x.Area.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }

    public async Task<List<Room>?> GetByWorkspaceIdAsync(string id)
    {
        try
        {
            return await _context.Room
                .Where(x => x.WorkspaceId == id && x.DeletedAt == null && x.Area.DeletedAt == null && x.Workspace.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }

    public async Task<Room?> GetJoinEventsSlotsByIdAsync(string id)
    {
        try
        {
            return await _context.Room
                .Include(x => x.Events.Where(y => y.DeletedAt == null))
                .Include(x => x.Slots.Where(y => y.DeletedAt == null))
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }
}
