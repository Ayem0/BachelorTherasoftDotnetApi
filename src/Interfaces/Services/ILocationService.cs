using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ILocationService
{
    Task<Response<LocationDto?>> GetByIdAsync(string id);
    Task<Response<LocationDto?>> CreateAsync(string workspaceId, string name, string? description, string? address, string? city, string? country);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<LocationDto?>> UpdateAsync(string id, string? newName, string? newDescription, string? newAddress, string? newCity, string? newCountry);
}
