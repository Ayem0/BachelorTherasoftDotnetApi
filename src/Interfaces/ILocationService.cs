using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface ILocationService
{
    Task<LocationDto?> GetByIdAsync(string id);
    Task<LocationDto?> CreateAsync(string workspaceId, string name, string? description, string? address, string? city, string? country);
    Task<bool> DeleteAsync(string id);
    Task<LocationDto?> UpdateAsync(string id, string? newName, string? newDescription, string? newAddress, string? newCity, string? newCountry);
}
