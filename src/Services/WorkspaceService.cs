using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
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

    public async Task<Response<WorkspaceDto?>> GetByIdAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return new Response<WorkspaceDto?>(success: false, errors: ["Workspace not found."]);

        return new Response<WorkspaceDto?>(success: true, content: new WorkspaceDto(workspace));
    }

    public async Task<Response<WorkspaceDto?>> CreateAsync(string userId, CreateWorkspaceRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new Response<WorkspaceDto?>(success: false, errors: ["User not found."]);

        var workspace = new Workspace(request.Name, request.Description);
        var member = new Member(user, workspace, Status.Accepted) {
            User = user,
            Workspace = workspace
        };

        workspace.Members.Add(member);
        await _workspaceRepository.CreateAsync(workspace);

        return new Response<WorkspaceDto?>(success: true, content: new WorkspaceDto(workspace));
    }

    public async Task<Response<string>> RemoveMemberAsync(string id, string memberId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return new Response<string>(success: false, errors: ["Workspace not found."]);

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return new Response<string>(success: false, errors: ["Member not found."]);

        var isContained = workspace.Members.Remove(member);
        if (isContained) {
            await _workspaceRepository.UpdateAsync(workspace);
            return new Response<string>(success: true, content: "Successfully removed member.");
        }

        return new Response<string>(success: false, errors: ["Workspace does not contain this member."]);
    }

    public async Task<Response<string>> AddMemberAsync(string id, string memberId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return new Response<string>(success: false, errors: ["Workspace not found."]);

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return new Response<string>(success: false, errors: ["Member not found."]);

        var isContained = workspace.Members.Contains(member);
        if (!isContained)
        {
            workspace.Members.Add(member);
            await _workspaceRepository.UpdateAsync(workspace);
            return new Response<string>(success: true, content: "Successfully invited member.");
        }

        return new Response<string>(
            success: false, 
            errors: member.Status == Status.Accepted ? ["User is already a member of this workspace."] : ["Already invited this member"]
        );
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return new Response<string>(success: false, errors: ["Workspace not found."]);

        await _workspaceRepository.DeleteAsync(workspace);
        return new Response<string>(success: true, content: "Successfully removed member.");
    }

    public async Task<Response<WorkspaceDto?>> UpdateAsync(string id, UpdateWorkspaceRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return new Response<WorkspaceDto?>(success: false, errors: ["At least one field is required."]);
        
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null ) return new Response<WorkspaceDto?>(success: false, errors: ["Workspace not found."]);

        workspace.Name = request.NewName ?? workspace.Name;
        workspace.Description = request.NewDescription ?? workspace.Description;

        await _workspaceRepository.UpdateAsync(workspace);
        return new Response<WorkspaceDto?>(success: true, content: new WorkspaceDto(workspace));
    }
}
