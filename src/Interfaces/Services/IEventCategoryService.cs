using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventCategoryService
{
    Task<ActionResult<EventCategoryDto>> GetByIdAsync(string id);
    Task<ActionResult<EventCategoryDto>> CreateAsync(string workspaceId, string name, string icon, string color);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<EventCategoryDto>> UpdateAsync(string id, string? newName, string? newIcon);
}
