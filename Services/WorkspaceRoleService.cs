using System;
using BachelorTherasoftDotnetApi.Base;
using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.Services;

public class WorkspaceRoleService : IWorkspaceRoleService
{
    private readonly WorkspaceRoleRepository _workspaceRoleRepository;
     private readonly WorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;

    public WorkspaceRoleService(WorkspaceRoleRepository workspaceRoleRepository, UserManager<User> userManager, WorkspaceRepository workspaceRepository)
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

    public async Task<WorkspaceRoleDto?> CreateAsync(string name, string workspaceId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

        if (workspace == null) return null;

        var workspaceRole = new WorkspaceRole {
            Name = name,
            WorkspaceId = workspace.Id,
            Workspace = workspace
        };

        await _workspaceRoleRepository.CreateAsync(workspaceRole);

        var workspaceRoleDto = new WorkspaceRoleDto {
            Id = workspaceRole.Id,
            Name = workspaceRole.Name
        };

        return  workspaceRoleDto;
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
    public async Task<bool> UpdateAsync(string id, string newName)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return false;

        workspaceRole.Name = newName;

        await _workspaceRoleRepository.UpdateAsync(workspaceRole);

        return true;
    }
}
