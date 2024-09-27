using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceRoleService : IWorkspaceRoleService
{
    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;
     private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    public WorkspaceRoleService(IWorkspaceRoleRepository workspaceRoleRepository, UserManager<User> userManager,IWorkspaceRepository workspaceRepository)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
        _userManager = userManager;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<bool> AddRoleToMemberAsync(string id, string userId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var isContained = workspaceRole.Users.Contains(user);
        if (!isContained) {
            workspaceRole.Users.Add(user);
            await _workspaceRoleRepository.UpdateAsync(workspaceRole);
        }
        return !isContained;
    }

    public async Task<WorkspaceRoleDto?> CreateAsync(string workspaceId, string name, string? description)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var workspaceRole = new WorkspaceRole(workspace, name, description){ Workspace = workspace };
        await _workspaceRoleRepository.CreateAsync(workspaceRole);

        return new WorkspaceRoleDto(workspaceRole);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return false;

        await _workspaceRoleRepository.DeleteAsync(workspaceRole);
        return true;
    }

    public async Task<WorkspaceRoleDto?> GetByIdAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return null;

        return new WorkspaceRoleDto(workspaceRole);
    }

    public async Task<bool> RemoveRoleFromMemberAsync(string id, string userId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;
        
        var isContained =  workspaceRole.Users.Remove(user);
        if (isContained) await _workspaceRoleRepository.UpdateAsync(workspaceRole);
            
        return isContained;
    }

    public async Task<WorkspaceRoleDto?> UpdateAsync(string id, string? newName, string? newDescription)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null || (newName == null && newDescription == null)) return null;

        workspaceRole.Name = newName ?? workspaceRole.Name;
        workspaceRole.Description = newDescription ?? workspaceRole.Description;

        await _workspaceRoleRepository.UpdateAsync(workspaceRole);
        return new WorkspaceRoleDto(workspaceRole);
    }
}
