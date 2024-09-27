using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IWorkspaceRoleService
{
    Task<WorkspaceRoleDto?> GetByIdAsync(string id);
    Task<WorkspaceRoleDto?> CreateAsync(string workspaceId, string name, string? description);
    Task<bool> AddRoleToMemberAsync(string id, string userID);
    Task<bool> RemoveRoleFromMemberAsync(string id, string userID);
    Task<bool> DeleteAsync(string id);
    Task<WorkspaceRoleDto?> UpdateAsync(string id, string? newName, string? newDescription);
}
