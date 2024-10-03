using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class RoomRepository : BaseMySqlRepository<Room>, IRoomRepository
{
    public RoomRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Room?> GetByIdAsync(string id)
    {
        return await _context.Room
            .Include(r => r.Area)
            .Include(r => r.Slots)
                .ThenInclude(s => s.EventCategories.Where(x => x.DeletedAt == null)) 
            .Include(r => r.Events)
            .Where(r => r.Id == id && r.DeletedAt == null && r.Area.DeletedAt == null && r.Slots.All(s => s.DeletedAt == null) && r.Events.All(e => e.DeletedAt == null))
            .FirstOrDefaultAsync();
    }
}
