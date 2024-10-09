using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventCategoryService
{
    Task<Response<EventCategoryDto?>> GetByIdAsync(string id);
    Task<Response<EventCategoryDto?>> CreateAsync(string workspaceId, string name, string icon, string color);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<EventCategoryDto?>> UpdateAsync(string id, string? newName, string? newIcon);
}
