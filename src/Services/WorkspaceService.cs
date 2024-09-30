using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly UserManager<User> _userManager;
    public WorkspaceService(IWorkspaceRepository workspaceRepository, UserManager<User> userManager, IMemberRepository memberRepository)
    {
        _workspaceRepository = workspaceRepository;
        _userManager = userManager;
        _memberRepository = memberRepository;
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

        var workspace = new Workspace(name, description);
        var member = new Member(user, workspace, Status.Accepeted) {
            User = user,
            Workspace = workspace
        };

        workspace.Members.Add(member);
        await _workspaceRepository.CreateAsync(workspace);

        return new WorkspaceDto(workspace);
    }

    public async Task<bool> RemoveMemberAsync(string id, string memberId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return false;

        var isContained = workspace.Members.Remove(member);
        if (isContained) await _workspaceRepository.UpdateAsync(workspace);

        return isContained;
    }

    public async Task<bool> AddMemberAsync(string id, string memberId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return false;

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return false;

        var isContained = workspace.Members.Contains(member);
        if (!isContained)
        {
            workspace.Members.Add(member);
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
