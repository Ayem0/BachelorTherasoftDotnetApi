using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IAreaService
{
    Task<AreaDto?> GetByIdAsync(string id);
    Task<AreaDto?> CreateAsync(string locationId, string name, string? description);
    Task<bool> DeleteAsync(string id);
    Task<AreaDto?> UpdateAsync(string id, string? newName, string? newDescription);
}
