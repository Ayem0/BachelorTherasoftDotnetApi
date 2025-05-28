using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(MySqlDbContext context, ILogger<Room> logger) : base(context, logger)
    {
    }

    public async Task<List<Room>> GetByAreaIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Where(x => x.AreaId == id)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }

    public async Task<List<Room>> GetByWorkspaceIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Where(x => x.WorkspaceId == id)
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
            return await _dbSet
                .Include(x => x.Events)
                .Include(x => x.Slots)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }

    public async Task<Room?> GetJoinEventsSlotsByRangeAndIdAsync(string id, DateTime start, DateTime end)
    {
        try
        {
            return await _dbSet
                .Include(x => x.Events.Where(e => e.StartDate < end && start > e.EndDate))
                .Include(x => x.Slots.Where(s => s.StartDate < end && start > s.EndDate))
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }
}
