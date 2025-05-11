using System;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class SlotRepository : BaseRepository<Slot>, ISlotRepository
{
    public SlotRepository(MySqlDbContext context, ILogger<Slot> logger) : base(context, logger)
    {
    }

    public async Task<List<Slot>> GetRepetitionsById(string id)
    {
        return await _dbSet
            .Include(r => r.EventCategories)
            .Where(r => r.MainSlotId == id && r.DeletedAt == null && r.EventCategories.All(s => s.DeletedAt == null))
            .ToListAsync();
    }

    public async Task<List<Slot>> GetByWorkpaceIdAsync(string id)
    {
        return await _dbSet
            .Where(x => x.WorkspaceId == id && x.Workspace.DeletedAt == null && x.DeletedAt == null && x.MainSlotId == null)
            .ToListAsync();
    }
}
