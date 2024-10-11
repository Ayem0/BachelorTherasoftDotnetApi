using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult> AddRoleToMemberAsync(string id, string memberId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return new NotFoundObjectResult("Workspace role not found.");

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null) return new NotFoundObjectResult("Member not found.");


        if (!workspaceRole.Members.Contains(member))
        {
            workspaceRole.Members.Add(member);
            await _workspaceRoleRepository.UpdateAsync(workspaceRole);
            return new OkObjectResult("Successfully added role to member.");
        }
        return new BadRequestObjectResult("Member already has this role.");
    }

    public async Task<ActionResult<WorkspaceRoleDto>> CreateAsync(CreateWorkspaceRoleRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        var workspaceRole = new WorkspaceRole(workspace, request.Name, request.Description) { Workspace = workspace };
        await _workspaceRoleRepository.CreateAsync(workspaceRole);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "WorkspaceRole", 
            routeValues: new { id = workspaceRole.Id }, 
            value: new WorkspaceRoleDto(workspaceRole)
        );  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return new NotFoundObjectResult("Workspace role not found.");

        await _workspaceRoleRepository.DeleteAsync(workspaceRole);
        return new OkObjectResult("Successfully deleted workspace role.");
    }

    public async Task<ActionResult<WorkspaceRoleDto>> GetByIdAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return new NotFoundObjectResult("Workspace role not found.");

        return new OkObjectResult(new WorkspaceRoleDto(workspaceRole));
    }

    public async Task<ActionResult> RemoveRoleFromMemberAsync(string id, string memberId)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null) return  new NotFoundObjectResult("Workspace role not found.");

        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)return  new NotFoundObjectResult("Member role not found.");

        var isContained = workspaceRole.Members.Remove(member);
        if (isContained) {
            await _workspaceRoleRepository.UpdateAsync(workspaceRole);
            return new OkObjectResult("Successfully removed role from member.");
        }
        return new BadRequestObjectResult("Member does not have this role.");
    }

    public async Task<ActionResult<WorkspaceRoleDto>> UpdateAsync(string id, UpdateWorkspaceRoleRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return new BadRequestObjectResult("At least one field is required.");

        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
        if (workspaceRole == null ) return new NotFoundObjectResult("Member not found.");

        workspaceRole.Name = request.NewName ?? workspaceRole.Name;
        workspaceRole.Description = request.NewDescription ?? workspaceRole.Description;

        await _workspaceRoleRepository.UpdateAsync(workspaceRole);
        return new OkObjectResult(new WorkspaceRoleDto(workspaceRole));
    }
}
