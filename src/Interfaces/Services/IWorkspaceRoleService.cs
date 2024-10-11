using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceRoleService
{
    Task<ActionResult<WorkspaceRoleDto>> GetByIdAsync(string id);
    Task<ActionResult<WorkspaceRoleDto>> CreateAsync(CreateWorkspaceRoleRequest request);
    Task<ActionResult> AddRoleToMemberAsync(string id, string userId);
    Task<ActionResult> RemoveRoleFromMemberAsync(string id, string userId);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<WorkspaceRoleDto>> UpdateAsync(string id, UpdateWorkspaceRoleRequest request);
}
