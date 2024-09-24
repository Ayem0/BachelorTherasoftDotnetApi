using BachelorTherasoftDotnetApi.Dtos;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IWorkspaceService
{
    Task<WorkspaceDto?> GetByIdAsync(string id);
    Task<WorkspaceDto?> CreateAsync(string name, string? userId);
    Task<bool> AddMemberAsync(string id, string userID);
    Task<bool> RemoveMemberAsync(string id, string userID);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string newName);
}
