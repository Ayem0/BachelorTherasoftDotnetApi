using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantCategoryService
{
    Task<ActionResult<ParticipantCategoryDto>> GetByIdAsync(string id);
    Task<ActionResult<ParticipantCategoryDto>> CreateAsync(string workspaceId, string name, string icon);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<ParticipantCategoryDto>> UpdateAsync(string id, string? newName, string? newIcon);
}
