using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(MySqlDbContext context, ILogger<Event> logger) : base(context, logger)
    {
    }

    public async Task<Event?> GetByIdJoinRelationsAsync(string id)
    {
        return await _dbSet
            .Include(e => e.Workspace)
            .Include(e => e.Room)
            .Include(e => e.EventCategory)
            .Include(e => e.Users)
            .Include(e => e.Participants)
            .Include(e => e.Tags)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Event?> GetByIdJoinWorkspaceAsync(string id)
    {
        return await _dbSet
            .Include(e => e.Room)
                .ThenInclude(r => r.Area)
                    .ThenInclude(a => a.Location)
                        .ThenInclude(l => l.Workspace)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Event>> GetByRangeAndUserIdAsync(string id, DateTime start, DateTime end)
    {
        return await _dbSet
            .Include(x => x.Users)
                .ThenInclude(eu => eu.User)
            .Include(x => x.Participants)
            .Include(x => x.Tags)
            .Include(x => x.EventCategory)
            .Include(x => x.Room)
            .Include(x => x.Workspace)
            .Where(e => ((e.StartDate <= end && e.EndDate >= end)
            || (e.StartDate > start && e.EndDate < end)
            || (e.StartDate < start && e.EndDate > start && e.EndDate < end)
            || (e.StartDate > start && e.EndDate > end && e.StartDate < end))

            //             // event with same date or starting before and ending after
            // (event.startDate <= dateRange.start && event.endDate >= dateRange.end) ||
            // // event starting after and ending before
            // (event.startDate > dateRange.start && event.endDate < dateRange.end) ||
            // // event starting before and ending before
            // (event.startDate < dateRange.start &&
            //   event.endDate > dateRange.start &&
            //   event.endDate < dateRange.end) ||
            // // event starting after and ending after
            // (event.startDate > dateRange.start &&
            //   event.endDate > dateRange.end &&
            //   event.startDate < dateRange.end)


            && e.Users.Any(x => x.UserId == id)).ToListAsync();
    }

    public async Task<List<Event>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end)
    {
        return await _dbSet.Where(e => e.StartDate < end && start > e.EndDate && e.RoomId == id).ToListAsync();
    }

    public Task<List<Event>> GetEventsByUserIdsAndRoomIdAsync(List<string> userIds, string roomId, DateTime start, DateTime end)
    {
        return _dbSet
             .Include(e => e.Users)
             .Where(e => e.StartDate < end && start > e.EndDate && (e.Users.Select(x => x.UserId).Any(id => userIds.Contains(id)) || e.RoomId == roomId)).ToListAsync();
    }
}
