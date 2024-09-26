using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Interfaces;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
     public RoomRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Room?> GetByIdAsync(string id)
    {
        return await _context.Room
            .Include(w => w.Area)
            .Include(w => w.Slots)
            .Include(w => w.Events)
            .Where(w => w.Id == id && w.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
}
