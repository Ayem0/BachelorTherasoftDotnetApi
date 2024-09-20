using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Databases;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.Repositories;

public class AreaRepository : IAreaRepository
{
    private readonly MySqlDbContext _context;
    private readonly LocationRepository _locationRepository;

    public AreaRepository(MySqlDbContext context, LocationRepository locationRepository)
    {
        _context = context;
        _locationRepository = locationRepository;
    }

    public async Task CreateAsync(string name, string locationId)
    {
        var location = await _locationRepository.GetLocationAsync(locationId);

        if (location == null) return;

        var area = new Area{
            Location = location,
            LocationId = location.Id,
            Name = name,
        };

        await _context.Area.AddAsync(area);
    }

    public async Task DeleteAsync(string areaId)
    {
        var area = await GetAreaAsync(areaId);

        if (area == null) return;

        _context.Area.Remove(area);
        await _context.SaveChangesAsync();
    }

    public async Task<Area?> GetAreaAsync(string areaId)
    {
        var area = await _context.Area.Where(x => x.Id == areaId && x.DeletedAt == null).FirstAsync();
        return area;
    }

    public async Task<List<Area>?> GetAreasAsync(string[] areasId)
    {
        var areas = await _context.Area.Where(x => areasId.Contains(x.Id) && x.DeletedAt == null).ToListAsync();
        return areas;
    }

    public async Task UpdateAsync(string areaId, string name)
    {
        var area = await GetAreaAsync(areaId);

        if ( area == null) return;

        area.Name = name;

        _context.Area.Update(area);
        await _context.SaveChangesAsync();        
    }
}
