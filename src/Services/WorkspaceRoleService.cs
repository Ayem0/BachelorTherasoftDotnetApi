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
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;

    public WorkspaceRoleService(
        IWorkspaceRoleRepository workspaceRoleRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        ISocketService socket)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _socket = socket;
        _workspaceRepository = workspaceRepository;
    }
    // var workspace = await _cache.GetOrSetAsync(
    //     CacheKeys.Workspace(workspaceId),
    //     () => _workspaceRepository.GetByIdAsync(workspaceId),
    //     ttl
    // ) ?? throw new NotFoundException("Workspace", workspaceId);
    // await _cache.SetAsync(CacheKeys.WorkspaceRole(workspaceId, created.Id), created, ttl);
    // await _cache.DeleteAsync(CacheKeys.WorkspaceRoles(workspaceId));

    public async Task<WorkspaceRoleDto> CreateAsync(CreateWorkspaceRoleRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var WorkspaceRole = new WorkspaceRole(workspace, req.Name, req.Description) { Workspace = workspace };
        var created = await _workspaceRoleRepository.CreateAsync(WorkspaceRole);
        var dto = _mapper.Map<WorkspaceRoleDto>(created);
        await _socket.NotififyGroup(req.WorkspaceId, "WorkspaceRoleCreated", dto);
        return dto;
    }

    public async Task<WorkspaceRoleDto> UpdateAsync(string id, UpdateWorkspaceRoleRequest req)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id) ?? throw new NotFoundException("WorkspaceRole", id);

        workspaceRole.Name = req.Name ?? workspaceRole.Name;
        workspaceRole.Description = req.Description ?? workspaceRole.Description;

        var updated = await _workspaceRoleRepository.UpdateAsync(workspaceRole);
        var dto = _mapper.Map<WorkspaceRoleDto>(updated);
        await _socket.NotififyGroup(updated.WorkspaceId, "WorkspaceRoleUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id) ?? throw new NotFoundException("WorkspaceRole", id);
        var success = await _workspaceRoleRepository.DeleteAsync(workspaceRole);
        if (success)
        {
            await _socket.NotififyGroup(workspaceRole.WorkspaceId, "WorkspaceRoleDeleted", id);
        }
        return success;
    }

    public async Task<WorkspaceRoleDto?> GetByIdAsync(string id)
    => _mapper.Map<WorkspaceRoleDto?>(await _workspaceRoleRepository.GetByIdAsync(id));

    public async Task<List<WorkspaceRoleDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<WorkspaceRoleDto>>(await _workspaceRoleRepository.GetByWorkspaceIdAsync(workspaceId));

    // public async Task<ActionResult> AddRoleToMemberAsync(string id, string userId)
    // {
    //     var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id);
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

    // public async Task<ActionResult> RemoveRoleFromMemberAsync(string id, string userId)
    // {
    //     var workspaceRole = await _workspaceRoleRepository.GetByIdAsync(id); // TODO a changer par une requete qui join les users
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
}
