using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface ILocationRepository
{
    Task CreateAsync(string workspaceId, string name, string? address, string? city, string? country);
    Task UpdateAsync(string locationId, string? name, string? address, string? city, string? country);
    Task<Location?> GetLocationAsync(string locationId);
    Task<List<Location>?> GetLocationsAsync(string[] locationIds);
    Task DeleteAsync(string locationId);
}
