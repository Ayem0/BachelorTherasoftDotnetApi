using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceService
{
    Task<WorkspaceDto> GetByIdAsync(string id);
    Task<List<WorkspaceDto>> GetByUserIdAsync(string id);
    Task<WorkspaceDto> CreateAsync(string userId, CreateWorkspaceRequest request);
    // Task<bool> RemoveMemberAsync(string id, string userId);
    Task<bool> DeleteAsync(string id);
    Task<WorkspaceDto> UpdateAsync(string id, UpdateWorkspaceRequest request);
    Task<List<MemberDto>> GetMembersByIdAsync(string id);
}
