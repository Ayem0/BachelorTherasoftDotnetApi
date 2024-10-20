using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
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

    // public async Task<ActionResult> AddRoleToMemberAsync(string id, string userId)
    // {
    //     var workspaceRole = await _workspaceRoleRepository.GetEntityByIdAsync(id);
    //     if (workspace == null) return Response.NotFound(id, "WorkspaceRole");

    //     var user = await _userManager.FindByIdAsync(userId);
    //     if (user == null) return Response.NotFound(userId, "User");


    //     if (!workspace.Users.Contains(user))
    //     {
    //         workspace.Users.Add(user);
    //         var res2 = await _workspaceRoleRepository.UpdateAsync(workspace);
    //         if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
            
    //         return Response.Ok("Successfully added role to member.");
    //     }
    //     return Response.BadRequest("Member already has this role.", null);
    // }

    public async Task<WorkspaceRoleDto> CreateAsync(CreateWorkspaceRoleRequest request)
    {
        var workspace = await _workspaceRepository.GetEntityByIdAsync(request.WorkspaceId) ?? throw new NotFoundException("Workspace", request.WorkspaceId);

        var workspaceRole = new WorkspaceRole(workspace, request.Name, request.Description) { Workspace = workspace };

        await _workspaceRoleRepository.CreateAsync(workspaceRole);

        return _mapper.Map<WorkspaceRoleDto>(workspaceRole);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _workspaceRoleRepository.DeleteAsync(id);
    }

    public async Task<WorkspaceRoleDto> GetByIdAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("WorkspaceRole", id);

        return _mapper.Map<WorkspaceRoleDto>(workspaceRole);
    }

    // public async Task<ActionResult> RemoveRoleFromMemberAsync(string id, string userId)
    // {
    //     var workspaceRole = await _workspaceRoleRepository.GetEntityByIdAsync(id); // TODO a changer par une requete qui join les users
    //     if (workspaceRole == null) return Response.NotFound(id, "WorkspaceRole");

    //     var user = await _userManager.FindByIdAsync(userId);
    //     if (user == null )return Response.NotFound(userId, "User");

    //     var isContained = res.Data.Users.Remove(user);
    //     if (isContained) {
    //         var res2 = await _workspaceRoleRepository.UpdateAsync(res.Data);
    //         if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

    //         return Response.Ok("Successfully removed role from member.");
    //     }
    //     return Response.BadRequest("Member does not have this role.", null);
    // }

    public async Task<WorkspaceRoleDto> UpdateAsync(string id, UpdateWorkspaceRoleRequest request)
    {
        var workspaceRole = await _workspaceRoleRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("WorkspaceRole", id);

        workspaceRole.Name = request.NewName ?? workspaceRole.Name;
        workspaceRole.Description = request.NewDescription ?? workspaceRole.Description;

        await _workspaceRoleRepository.UpdateAsync(workspaceRole);

        return _mapper.Map<WorkspaceRoleDto>(workspaceRole);
    }
}
