using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class AreaRepository : BaseRepository<Area>
{
    public AreaRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Area?> GetByIdAsync(string id)
    {
        return await _context.Area
            .Include(w => w.Rooms)
            .Include(w => w.Location)
            .Where(w => w.Id == id && w.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
}
