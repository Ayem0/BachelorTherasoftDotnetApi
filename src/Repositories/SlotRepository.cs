using System;
using System.Security.Cryptography.X509Certificates;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class SlotRepository : BaseMySqlRepository<Slot>, ISlotRepository
{
    public SlotRepository(MySqlDbContext context) : base(context)
    {   
    }

    public async new Task<Slot?> GetByIdAsync(string id)
    {
        return await _context.Slot
            .Include(r => r.EventCategories)
            .Where(r => r.Id == id && r.DeletedAt == null && r.EventCategories.All(s => s.DeletedAt == null))
            .FirstOrDefaultAsync();
    }
    public async Task<List<Slot>> GetRepetitionsById(string id)
    {
        return await _context.Slot
            .Include(r => r.EventCategories)
            .Where(r => r.MainSlotId == id && r.DeletedAt == null && r.EventCategories.All(s => s.DeletedAt == null))
            .ToListAsync();
    }
}
