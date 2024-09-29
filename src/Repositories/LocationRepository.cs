using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;


namespace BachelorTherasoftDotnetApi.src.Repositories;

public class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    public LocationRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Location?> GetByIdAsync(string id)
    {
        return await _context.Location
            .Include(l => l.Areas)
            .Include(l => l.Workspace)
            .Where(l => l.Id == id && l.DeletedAt == null && l.Areas.All(a => a.DeletedAt == null) && l.Workspace.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
}
