using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventCategoryService
{
    Task<EventCategoryDto?> GetByIdAsync(string id);
    Task<EventCategoryDto?> CreateAsync(string workspaceId, string name, string icon, string color);
    Task<bool> DeleteAsync(string id);
    Task<EventCategoryDto?> UpdateAsync(string id, string? newName, string? newIcon);
}
