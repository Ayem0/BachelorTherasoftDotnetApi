using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class AreaRepository : BaseMySqlRepository<Area>, IAreaRepository
{
    public AreaRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Area?> GetByIdAsync(string id)
    {
        return await _context.Area
            .Include(w => w.Rooms)
            .Include(w => w.Location)
            .Where(w => w.Id == id && w.DeletedAt == null && w.Location.DeletedAt == null && w.Rooms.All(r => r.DeletedAt == null))
            .FirstOrDefaultAsync();
    }
}
