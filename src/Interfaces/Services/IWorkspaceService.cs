using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceService
{
    Task<WorkspaceDto?> GetByIdAsync(string id);
    Task<WorkspaceDto?> CreateAsync(string userId, string name, string? description);
    Task<bool> AddMemberAsync(string id, string userID);
    Task<bool> RemoveMemberAsync(string id, string userID);
    Task<bool> DeleteAsync(string id);
    Task<WorkspaceDto?> UpdateAsync(string id, string? newName, string? description);
}
