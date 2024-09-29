using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    public WorkspaceService(IWorkspaceRepository workspaceRepository, UserManager<User> userManager)
    {
        _workspaceRepository = workspaceRepository;
        _userManager = userManager;
    }

    public async Task<WorkspaceDto?> GetByIdAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return null;

        return new WorkspaceDto(workspace);
    }

    public async Task<WorkspaceDto?> CreateAsync(string userId, string name, string? description)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var workspace = new Workspace(name, description, [user]);
        await _workspaceRepository.CreateAsync(workspace);

        return new WorkspaceDto(workspace);
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

        var isContained = workspace.Users.Contains(user);
        if (!isContained)
        {
            workspace.Users.Add(user);
            await _workspaceRepository.UpdateAsync(workspace);
        }
        return !isContained;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        await _workspaceRepository.DeleteAsync(workspace);
        return true;
    }

    public async Task<WorkspaceDto?> UpdateAsync(string id, string? newName, string? newDescription)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null || (newDescription == null && newName == null)) return null;

        workspace.Name = newName ?? workspace.Name;
        workspace.Description = newDescription ?? workspace.Description;

        await _workspaceRepository.UpdateAsync(workspace);
        return new WorkspaceDto(workspace);
    }
}
