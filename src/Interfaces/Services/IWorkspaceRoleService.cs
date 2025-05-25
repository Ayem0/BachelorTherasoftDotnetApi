using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceRoleService
{
    Task<WorkspaceRoleDto?> GetByIdAsync(string workspaceId, string id);
    Task<WorkspaceRoleDto> CreateAsync(string workspaceId, CreateWorkspaceRoleRequest request);
    // Task< AddRoleToMemberAsync(string id, string userId);
    // Task< RemoveRoleFromMemberAsync(string id, string userId);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<WorkspaceRoleDto> UpdateAsync(string workspaceId, string id, UpdateWorkspaceRoleRequest request);
    Task<List<WorkspaceRoleDto>> GetByWorkspaceIdAsync(string id);
}
