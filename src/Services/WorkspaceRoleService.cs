using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
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

    public async Task<Response<string>> AddRoleToMemberAsync(string id, string memberId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return new Response<string>(success: false, errors: ["Workspace role not found."]);

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return new Response<string>(success: false, errors: ["Member not found."]);


        if (!workspaceRole.Members.Contains(member))
        {
            workspaceRole.Members.Add(member);
            await _workspaceRoleRepository.UpdateAsync(workspaceRole);
            return new Response<string>(success: true, content: "Successfully added role to member.");
        }
        return new Response<string>(success: false, content: "Member already has this role.");
    }

    public async Task<Response<WorkspaceRoleDto?>> CreateAsync(CreateWorkspaceRoleRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new Response<WorkspaceRoleDto?>(success: false, errors: ["Workspace not found."]);

        var workspaceRole = new WorkspaceRole(workspace, request.Name, request.Description) { Workspace = workspace };
        await _workspaceRoleRepository.CreateAsync(workspaceRole);

        return new Response<WorkspaceRoleDto?>(success: true, content: new WorkspaceRoleDto(workspaceRole));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return new Response<string>(success: false, errors: ["Workspace role not found."]);

        await _workspaceRoleRepository.DeleteAsync(workspaceRole);
        return new Response<string>(success: true, content: "Successfully deleted workspace role.");
    }

    public async Task<Response<WorkspaceRoleDto?>> GetByIdAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return new Response<WorkspaceRoleDto?>(success: false, errors: ["Workspace role not found."]);

        return new Response<WorkspaceRoleDto?>(success: true, content: new WorkspaceRoleDto(workspaceRole));
    }
        zazazazazaza
    public async Task<Response<string>> RemoveRoleFromMemberAsync(string id, string memberId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return new Response<string>(success: true, errors: ["Workspace role not found."]);

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return new Response<string>(success: true, errors: ["Member not found."]);

        var isContained = workspaceRole.Members.Remove(member);
        if (isContained) await _workspaceRoleRepository.UpdateAsync(workspaceRole);
        if ()

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
