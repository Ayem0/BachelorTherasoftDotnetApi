using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventCategoryService
{
    Task<EventCategoryDto?> GetByIdAsync(string id);
    Task<EventCategoryDto> CreateAsync(CreateEventCategoryRequest req);
    Task<bool> DeleteAsync(string id);
    Task<EventCategoryDto> UpdateAsync(string id, UpdateEventCategoryRequest req);
    Task<List<EventCategoryDto>> GetByWorkspaceIdAsync(string workspaceId);
}
