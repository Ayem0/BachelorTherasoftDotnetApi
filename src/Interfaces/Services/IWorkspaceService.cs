using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceService
{
    Task<ActionResult<WorkspaceDto>> GetByIdAsync(string id);
    Task<ActionResult<WorkspaceDto>> CreateAsync(string userId, CreateWorkspaceRequest request);
    Task<ActionResult> RemoveMemberAsync(string id, string userID);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<WorkspaceDto>> UpdateAsync(string id, UpdateWorkspaceRequest request);
}
