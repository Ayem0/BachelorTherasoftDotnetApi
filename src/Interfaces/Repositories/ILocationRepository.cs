
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface ILocationRepository
{
    Task<Location> CreateAsync(Location Location);
    Task<Location?> GetByIdAsync(string id);
    Task<Location> UpdateAsync(Location Location);
    Task<bool> DeleteAsync(string id);
    Task<Location?> GetDetailsByIdAsync(string id);
    Task<Location[]> GetByWorkspaceIdAsync(string id);
}
