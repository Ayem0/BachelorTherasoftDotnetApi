using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceRoleService
{
    Task<Response<WorkspaceRoleDto?>> GetByIdAsync(string id);
    Task<Response<WorkspaceRoleDto?>> CreateAsync(CreateWorkspaceRoleRequest request);
    Task<Response<string>> AddRoleToMemberAsync(string id, string userId);
    Task<Response<string>> RemoveRoleFromMemberAsync(string id, string userId);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<WorkspaceRoleDto?>> UpdateAsync(string id, UpdateWorkspaceRoleRequest request);
}
