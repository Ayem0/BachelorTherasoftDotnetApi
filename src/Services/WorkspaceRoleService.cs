using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceRoleService : IWorkspaceRoleService
{
    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    public WorkspaceRoleService(IWorkspaceRoleRepository workspaceRoleRepository, UserManager<User> userManager, IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
        _userManager = userManager;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult> AddRoleToMemberAsync(string id, string userId)
    {
        var res = await _workspaceRoleRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, "WorkspaceRole");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Response.NotFound(userId, "User");


        if (!res.Data.Users.Contains(user))
        {
            res.Data.Users.Add(user);
            var res2 = await _workspaceRoleRepository.UpdateAsync(res.Data);
            if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
            
            return Response.Ok("Successfully added role to member.");
        }
        return Response.BadRequest("Member already has this role.", null);
    }

    public async Task<ActionResult<WorkspaceRoleDto>> CreateAsync(CreateWorkspaceRoleRequest request)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(request.WorkspaceId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(request.WorkspaceId, "Workspace");

        var workspaceRole = new WorkspaceRole(res.Data, request.Name, request.Description) { Workspace = res.Data };

        var res2 = await _workspaceRoleRepository.CreateAsync(workspaceRole);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.CreatedAt(_mapper.Map<WorkspaceRoleDto>(workspaceRole));
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _workspaceRoleRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        return Response.NoContent();
    }

    public async Task<ActionResult<WorkspaceRoleDto>> GetByIdAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync<WorkspaceRoleDto>(id);
        if (workspaceRole == null) return Response.NotFound(id, "WorkspaceRole");

        return Response.Ok(workspaceRole);
    }

    public async Task<ActionResult> RemoveRoleFromMemberAsync(string id, string userId)
    {
        var res = await _workspaceRoleRepository.GetEntityByIdAsync(id); // TODO a changer par une requete qui join les users
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, "WorkspaceRole");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null )return Response.NotFound(userId, "User");

        var isContained = res.Data.Users.Remove(user);
        if (isContained) {
            var res2 = await _workspaceRoleRepository.UpdateAsync(res.Data);
            if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

            return Response.Ok("Successfully removed role from member.");
        }
        return Response.BadRequest("Member does not have this role.", null);
    }

    public async Task<ActionResult<WorkspaceRoleDto>> UpdateAsync(string id, UpdateWorkspaceRoleRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return Response.BadRequest("At least one field is required.", null);

        var res = await _workspaceRoleRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null ) return Response.NotFound(id,"Workspace Role");

        res.Data.Name = request.NewName ?? res.Data.Name;
        res.Data.Description = request.NewDescription ?? res.Data.Description;

        var res2 = await _workspaceRoleRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.Ok(_mapper.Map<WorkspaceRoleDto>(res.Data));
    }
}
