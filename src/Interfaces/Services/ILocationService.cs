using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ILocationService
{
    Task<LocationDto?> GetByIdAsync(string workspaceId, string id);
    Task<List<LocationDto>> GetByWorkspaceIdAsync(string id);
    Task<LocationDto> CreateAsync(string workspaceId, CreateLocationRequest req);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<LocationDto> UpdateAsync(string workspaceId, string id, UpdateLocationRequest req);
}
