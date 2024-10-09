using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceService
{
    Task<Response<WorkspaceDto?>> GetByIdAsync(string id);
    Task<Response<WorkspaceDto?>> CreateAsync(string userId, CreateWorkspaceRequest request);
    Task<Response<string>> AddMemberAsync(string id, string userID);
    Task<Response<string>> RemoveMemberAsync(string id, string userID);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<WorkspaceDto?>> UpdateAsync(string id, UpdateWorkspaceRequest request);
}
