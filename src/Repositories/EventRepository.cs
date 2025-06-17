using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
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
        try
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
        catch (Exception ex)
        {
            _logger.LogError("Error getting event by id '{id}' : {errorMessage}", id, ex.Message);
            throw new DbException(DbAction.Read, "Event", id);
        }
    }


    public async Task<List<Event>> GetByRangeAndUserIdJoinRelationsAsync(string id, DateTime start, DateTime end)
    {
        try
        {
            return await _dbSet
            .Include(e => e.Users)
                .ThenInclude(eu => eu.User)
            .Include(e => e.Participants)
            .Include(e => e.Tags)
            .Include(e => e.EventCategory)
            .Include(e => e.Room)
            .Include(e => e.Workspace)
            .Where(e => ((e.StartDate <= end && e.EndDate >= end)
            || (e.StartDate > start && e.EndDate < end)
            || (e.StartDate < start && e.EndDate > start && e.EndDate < end)
            || (e.StartDate > start && e.EndDate > end && e.StartDate < end))
            && e.Users.Any(u => u.UserId == id)).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting events by range '{start}-{end}' and userId '{id}' : {errorMessage}", start, end, id, ex.Message);
            throw new DbException(DbAction.Read, "Event");
        }
    }

    public async Task<List<Event>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end)
    {
        try
        {
            return await _dbSet
            .Include(e => e.Users)
                .ThenInclude(eu => eu.User)
            .Include(e => e.Participants)
            .Include(e => e.Tags)
            .Include(e => e.EventCategory)
            .Include(e => e.Room)
            .Include(e => e.Workspace)
            .Where(e => ((e.StartDate <= end && e.EndDate >= end)
            || (e.StartDate > start && e.EndDate < end)
            || (e.StartDate < start && e.EndDate > start && e.EndDate < end)
            || (e.StartDate > start && e.EndDate > end && e.StartDate < end))
            && e.RoomId == id).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting events by range '{start}-{end}' and roomId '{id}' : {errorMessage}", start, end, id, ex.Message);
            throw new DbException(DbAction.Read, "Event");
        }
    }
}
