using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ILocationService
{
    Task<LocationDto?> GetByIdAsync(string id);
    Task<List<LocationDto>> GetByWorkspaceIdAsync(string id);
    Task<LocationDto> CreateAsync(CreateLocationRequest req);
    Task<bool> DeleteAsync(string id);
    Task<LocationDto> UpdateAsync(string id, UpdateLocationRequest req);
}
