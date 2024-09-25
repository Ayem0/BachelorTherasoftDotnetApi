using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IWorkspaceRoleService
{
    Task<WorkspaceRoleDto?> GetByIdAsync(string id);
    Task<WorkspaceRoleDto?> CreateAsync(string name, string workspaceId);
    Task<bool> AddRoleToMemberAsync(string id, string userID);
    Task<bool> RemoveRoleFromMemberAsync(string id, string userID);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string newName);
}
