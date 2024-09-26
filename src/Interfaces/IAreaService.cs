using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IAreaService
{
    Task<AreaDto?> GetByIdAsync(string id);
    Task<AreaDto?> CreateAsync(string locationId, string name, string? description);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string? newName, string? newDescription);
}
