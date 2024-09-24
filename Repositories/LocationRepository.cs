using BachelorTherasoftDotnetApi.Base;
using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.EntityFrameworkCore;


namespace BachelorTherasoftDotnetApi.Repositories;

public class LocationRepository : BaseRepository<Location>
{
    public LocationRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Location?> GetByIdAsync(string id)
    {
        return await _context.Location
            .Include(w => w.Areas)
            .Include(w => w.Workspace)
            .Where(w => w.Id == id && w.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
}
