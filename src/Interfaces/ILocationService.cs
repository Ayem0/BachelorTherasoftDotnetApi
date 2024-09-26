using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface ILocationService
{
    Task<LocationDto?> GetByIdAsync(string id);
    Task<LocationDto?> CreateAsync(string name, string workspaceId);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string newName);
}
