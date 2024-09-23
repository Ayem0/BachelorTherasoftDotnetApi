using System;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IWorkspaceService
{
    Task<Workspace?> GetWorkspaceByIdAsync(string id);
    Task CreateWorkspaceAsync(Workspace workspace);
    Task UpdateWorkspaceAsync(Workspace workspace);
    Task DeleteWorkspaceAsync(string id);
}
