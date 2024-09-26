using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IAreaService
{
    Task<AreaDto?> GetByIdAsync(string id);
    Task<AreaDto?> CreateAsync(string name, string locationId);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string newName);
}
