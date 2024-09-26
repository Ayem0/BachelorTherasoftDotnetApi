using System;
using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IEventCategoryService
{
    Task<EventCategoryDto?> GetByIdAsync(string id);
    Task<EventCategoryDto?> CreateAsync(string workspaceId, string name, string icon);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string? newName, string? newIcon);
}
