using BachelorTherasoftDotnetApi.Base;
using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IBaseRepository<Workspace> _workspaceRepository;
    private readonly UserManager<User> _userManager;
    public WorkspaceService(IBaseRepository<Workspace> workspaceRepository, UserManager<User> userManager)
    {
        _workspaceRepository = workspaceRepository;
        _userManager = userManager;
    }
    public async Task<WorkspaceDto?> GetByIdAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return null;

        var workspaceDto = new WorkspaceDto {
            Id = workspace.Id,
            Name = workspace.Name,
            Users = workspace.Users.Select(user => new UserDto{
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            }).ToList() 
        };
        return workspaceDto;
    }

    public async Task<WorkspaceDto?> CreateAsync(string name, string? userId) 
    {
        if (userId == null) return null;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var workspace = new Workspace { Name = name };
        workspace.Users.Add(user);

        await _workspaceRepository.CreateAsync(workspace);

        var workspaceDto = new WorkspaceDto {
            Id = workspace.Id,
            Name = workspace.Name
        };
        return workspaceDto;
    }

    public async Task<bool> RemoveMemberAsync(string id, string userId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var isContained = workspace.Users.Remove(user);

        if (isContained) await _workspaceRepository.UpdateAsync(workspace);

        return isContained;
    }

    public async Task<bool> AddMemberAsync(string id, string userId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        if (!workspace.Users.Contains(user)) {
            workspace.Users.Add(user);
            await _workspaceRepository.UpdateAsync(workspace);
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteAsync(string id)
    { 
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;
        await _workspaceRepository.DeleteAsync(workspace);
        return true;
    }

    public async Task<bool> UpdateAsync(string id, string newName) 
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;
        workspace.Name = newName;
        await _workspaceRepository.UpdateAsync(workspace);
        return true;
    }
}
