using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventCategoryService
{
    Task<EventCategoryDto?> GetByIdAsync(string workspaceId, string id);
    Task<EventCategoryDto> CreateAsync(string workspaceId, CreateEventCategoryRequest req);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<EventCategoryDto> UpdateAsync(string workspaceId, string id, UpdateEventCategoryRequest req);
    Task<List<EventCategoryDto>> GetByWorkspaceIdAsync(string workspaceId);
}
