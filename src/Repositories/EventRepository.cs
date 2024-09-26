using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class EventRepository : BaseRepository<Event>
{
     public EventRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Event?> GetByIdAsync(string id)
    {
        return await _context.Event
            .Include(w => w.Room)
            .Include(w => w.Users)
            .Include(w => w.Participants)
            .Include(w => w.Tags)
            .Include(w => w.EventCategory)
            .Where(w => w.Id == id && w.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
}
