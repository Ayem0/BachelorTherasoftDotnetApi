using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<WorkspaceDto>> GetByIdAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        return new OkObjectResult(new WorkspaceDto(workspace));
    }

    public async Task<ActionResult<WorkspaceDto>> CreateAsync(string userId, CreateWorkspaceRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new NotFoundObjectResult("User not found.");

        var workspace = new Workspace(request.Name, request.Description);
        var member = new Member(user, workspace) {
            User = user,
            Workspace = workspace
        };

        workspace.Members.Add(member);
        await _workspaceRepository.CreateAsync(workspace);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Workspace", 
            routeValues: new { id = workspace.Id }, 
            value: new WorkspaceDto(workspace)
        );    
    }

    public async Task<ActionResult> RemoveMemberAsync(string id, string memberId)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return new NotFoundObjectResult("Member not found.");

        var isContained = workspace.Members.Remove(member);
        if (isContained) {
            await _workspaceRepository.UpdateAsync(workspace);
            return new OkObjectResult("Successfully removed member.");
        }

        return new NotFoundObjectResult("Member not found.");
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        await _workspaceRepository.DeleteAsync(workspace);
        return new OkObjectResult("Successfully deleted workspace.");

    }

    public async Task<ActionResult<WorkspaceDto>> UpdateAsync(string id, UpdateWorkspaceRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return new BadRequestObjectResult("At least one field is required.");
        
        var workspace = await _workspaceRepository.GetByIdAsync(id);
        if (workspace == null ) return new NotFoundObjectResult("Workspace not found.");

        workspace.Name = request.NewName ?? workspace.Name;
        workspace.Description = request.NewDescription ?? workspace.Description;

        await _workspaceRepository.UpdateAsync(workspace);
        return new OkObjectResult(new WorkspaceDto(workspace));
    }
}
