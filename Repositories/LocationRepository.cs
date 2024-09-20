using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Databases;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly MySqlDbContext _context;
    private readonly WorkspaceRepository _workspaceRepository;

    public LocationRepository(MySqlDbContext context, WorkspaceRepository workspaceRepository)
    {
        _context = context;
        _workspaceRepository = workspaceRepository;
    }

    public async Task CreateAsync(string workspaceId, string name, string? address, string? city, string? country)
    {
        var workspace = await _workspaceRepository.GetWorkspaceAsync(workspaceId);

        if ( workspace == null ) return;

        var location = new Location{
            Name = name,
            Workspace = workspace,
            WorkspaceId = workspace.Id,
            Address = address,
            City = city,
            Country = country
        };

        await _context.Location.AddAsync(location);
    }

    public async Task DeleteAsync(string locationId)
    {
        var location = await GetLocationAsync(locationId);

        if (location == null) return;

        location.DeletedAt = DateTime.Now;

        _context.Location.Update(location);
        await _context.SaveChangesAsync();
    }

    public async Task<Location?> GetLocationAsync(string locationId)
    {
        var location = await _context.Location.Where(x => x.Id == locationId && x.DeletedAt == null).FirstAsync();
        return location;
    }

    public async Task<List<Location>?> GetLocationsAsync(string[] locationIds)
    {
        var locations = await _context.Location.Where(x => locationIds.Contains(x.Id)).ToListAsync();
        return locations;
    }

    public async Task UpdateAsync(string locationId, string? name, string? address, string? city, string? country)
    {
        var location = await GetLocationAsync(locationId);

        if (location == null) return;

        location.Name = name ?? location.Name;
        location.Address = address ?? location.Address;
        location.City = city ?? location.City;
        location.Country = country ?? location.Country;

        _context.Location.Update(location);
        await _context.SaveChangesAsync();
    }
}
