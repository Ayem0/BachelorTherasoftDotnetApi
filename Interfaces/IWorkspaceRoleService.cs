using System;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IWorkspaceRoleService
{
    Task<WorkspaceRole?> GetWorkspaceRoleByIdAsync(string id);
    Task CreateWorkspaceRoleAsync(WorkspaceRole workspaceRole);
    Task UpdateWorkspaceRoleAsync(WorkspaceRole workspaceRole);
    Task DeleteWorkspaceRoleAsync(string id);
}
