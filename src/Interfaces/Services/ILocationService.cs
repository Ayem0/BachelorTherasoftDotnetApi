using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ILocationService
{
    Task<ActionResult<LocationDto>> GetByIdAsync(string id);
    Task<ActionResult<LocationDto>> CreateAsync(string workspaceId, string name, string? description, string? address, string? city, string? country);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<LocationDto>> UpdateAsync(string id, string? newName, string? newDescription, string? newAddress, string? newCity, string? newCountry);
}
