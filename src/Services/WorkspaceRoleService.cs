using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceRoleService : IWorkspaceRoleService
{
    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMemberRepository _memberRepository;
    public WorkspaceRoleService(IWorkspaceRoleRepository workspaceRoleRepository, IMemberRepository memberRepository, IWorkspaceRepository workspaceRepository)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
        _memberRepository = memberRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<bool> AddRoleToMemberAsync(string id, string memberId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return false;

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return false;

        var isContained = workspaceRole.Members.Contains(member);
        if (!isContained)
        {
            workspaceRole.Members.Add(member);
            await _workspaceRoleRepository.UpdateAsync(workspaceRole);
        }
        return !isContained;
    }

    public async Task<WorkspaceRoleDto?> CreateAsync(string workspaceId, string name, string? description)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var workspaceRole = new WorkspaceRole(workspace, name, description) { Workspace = workspace };
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

    public async Task<bool> RemoveRoleFromMemberAsync(string id, string memberId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return false;

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return false;

        var isContained = workspaceRole.Members.Remove(member);
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
