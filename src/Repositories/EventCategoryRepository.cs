using System;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class EventCategoryRepository : BaseMySqlRepository<EventCategory>, IEventCategoryRepository
{
    public EventCategoryRepository(MySqlDbContext context) : base(context)
    {
    }
    
    public async Task<List<EventCategory>> GetByWorkpaceIdAsync(string id) {
        return await _context.EventCategory
            .Where(x => x.WorkspaceId == id && x.Workspace.DeletedAt == null && x.DeletedAt == null)
            .ToListAsync();
    }
}
