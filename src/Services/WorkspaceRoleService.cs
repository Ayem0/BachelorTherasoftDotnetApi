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

        if (!workspaceRole.Users.Contains(user)) {
            workspaceRole.Users.Add(user);
            await _workspaceRoleRepository.UpdateAsync(workspaceRole);
            return true;
        }

        return false;
    }

    public async Task<WorkspaceRoleDto?> CreateAsync(string workspaceId, string name, string? description)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

        if (workspace == null) return null;

        var workspaceRole = new WorkspaceRole {
            Name = name,
            WorkspaceId = workspace.Id,
            Workspace = workspace,
            Description = description
        };

        await _workspaceRoleRepository.CreateAsync(workspaceRole);

        var workspaceRoleDto = new WorkspaceRoleDto {
            Id = workspaceRole.Id,
            Name = workspaceRole.Name,
            Description = workspaceRole.Description
        };

        return workspaceRoleDto;
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

        var workspaceRoleDto = new WorkspaceRoleDto {
            Id = workspaceRole.Id,
            Name = workspaceRole.Name,
            Description = workspaceRole.Description,
            Users = workspaceRole.Users.Select(user => new UserDto {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            }).ToList()
        };

        return workspaceRoleDto;
    }

    public async Task<bool> RemoveRoleFromMemberAsync(string id, string userId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        if (workspaceRole.Users.Contains(user)) {
            workspaceRole.Users.Remove(user);
            await _workspaceRoleRepository.UpdateAsync(workspaceRole);
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateAsync(string id, string? newName, string? newDescription)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null || (newName == null && newDescription == null)) return false;

        workspaceRole.Name = newName ?? workspaceRole.Name;
        workspaceRole.Description = newDescription ?? workspaceRole.Description;

        await _workspaceRoleRepository.UpdateAsync(workspaceRole);

        return true;
    }
}
