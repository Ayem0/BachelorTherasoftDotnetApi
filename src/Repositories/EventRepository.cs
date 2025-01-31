using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class EventRepository : BaseMySqlRepository<Event>, IEventRepository
{
    public EventRepository(MySqlDbContext context) : base(context)
    {
    }

    public async Task<Event?> GetByIdJoinRelationsAsync(string id)
    {
        return await _context.Event
            .Include(e => e.Room)
            .Include(e => e.EventCategory)
            .Include(e => e.Users)
            .Include(e => e.Participants)
            .Include(e => e.Tags)
            .Where(e => e.Id == id && e.DeletedAt == null && e.Room.DeletedAt == null && e.EventCategory.DeletedAt == null && e.Users.All(u => u.DeletedAt == null) &&
                e.Participants.All(p => p.DeletedAt == null) && e.Tags.All(t => t.DeletedAt == null))
            .FirstOrDefaultAsync();
    }

    public async Task<Event?> GetByIdJoinWorkspaceAsync(string id)
    {
        return await _context.Event
            .Include(e => e.Room)
                .ThenInclude(r => r.Area)
                    .ThenInclude(a => a.Location)
                        .ThenInclude(l => l.Workspace)
            .Where(e => e.Id == id && e.DeletedAt == null && e.Room.DeletedAt == null && e.Room.Area.DeletedAt == null &&
                e.Room.Area.Location.DeletedAt == null)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Event>> GetByRangeAndUserIdAsync(string id, DateTime start, DateTime end)
    {
        return await _context.Event
            .Include(x => x.Users)
            .Where(e => e.StartDate >= start && e.EndDate <= end && e.Users.Select(x => x.UserId).Contains(id) && e.DeletedAt == null).ToListAsync();
    }

    public async Task<List<Event>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end)
    {
        return await _context.Event.Where(e => e.StartDate >= start && e.EndDate <= end && e.DeletedAt == null).ToListAsync();
    }
}
