using System;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Services;

public class WorkspaceRoleService : IWorkspaceRoleService
{
    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;

    public WorkspaceRoleService(IWorkspaceRoleRepository workspaceRoleRepository)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
    }

    public async Task CreateWorkspaceRoleAsync(WorkspaceRole workspaceRole)
    {
        await _workspaceRoleRepository.CreateAsync(workspaceRole);
    }

    public async Task DeleteWorkspaceRoleAsync(string id)
    {
        await _workspaceRoleRepository.DeleteAsync(id);
    }

    public async Task<WorkspaceRole?> GetWorkspaceRoleByIdAsync(string id)
    {
        return await _workspaceRoleRepository.GetByIdAsync(id);
    }

    public async Task UpdateWorkspaceRoleAsync(WorkspaceRole workspaceRole)
    {
        await _workspaceRoleRepository.UpdateAsync(workspaceRole);
    }
}
