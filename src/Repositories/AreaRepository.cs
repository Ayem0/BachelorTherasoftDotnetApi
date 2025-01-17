using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class AreaRepository : BaseMySqlRepository<Area>, IAreaRepository
{
    public AreaRepository(MySqlDbContext context) : base(context)
    {
    }

    public async Task<Area?> GetByIdJoinRoomsAsync(string id)
    {
        return await _context.Area
            .Include(w => w.Rooms)
            .Where(w => w.Id == id && w.DeletedAt == null && w.Location.DeletedAt == null && w.Location.Workspace.DeletedAt == null
                && w.Rooms.All(x => x.DeletedAt == null))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Area>> GetAreasByLocationIdAsync(string id)
    {
        return await _context.Area
            .Where(a => a.LocationId == id && a.DeletedAt == null)
            .ToListAsync();
    }
}
