using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;


namespace BachelorTherasoftDotnetApi.src.Repositories;

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
