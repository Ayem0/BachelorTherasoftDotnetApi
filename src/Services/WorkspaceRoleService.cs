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
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public WorkspaceRoleService(
        IWorkspaceRoleRepository workspaceRoleRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService socket)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _cache = cache;
        _socket = socket;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<WorkspaceRoleDto> CreateAsync(string workspaceId, CreateWorkspaceRoleRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var WorkspaceRole = new WorkspaceRole(workspace, req.Name, req.Description) { Workspace = workspace };

        var created = await _workspaceRoleRepository.CreateAsync(WorkspaceRole);
        var dto = _mapper.Map<WorkspaceRoleDto>(WorkspaceRole);

        await _socket.NotififyGroup(workspaceId, "WorkspaceRoleCreated", dto);
        await _cache.SetAsync(CacheKeys.WorkspaceRole(workspaceId, created.Id), created, ttl);
        await _cache.DeleteAsync(CacheKeys.WorkspaceRoles(workspaceId));

        return dto;
    }

    public async Task<WorkspaceRoleDto> UpdateAsync(string workspaceId, string id, UpdateWorkspaceRoleRequest req)
    {
        var key = CacheKeys.WorkspaceRole(workspaceId, id);
        var WorkspaceRole = await _cache.GetOrSetAsync(key, () => _workspaceRoleRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("WorkspaceRole", id);

        WorkspaceRole.Name = req.Name ?? WorkspaceRole.Name;
        WorkspaceRole.Description = req.Description ?? WorkspaceRole.Description;

        var updated = await _workspaceRoleRepository.UpdateAsync(WorkspaceRole);
        var dto = _mapper.Map<WorkspaceRoleDto>(updated);

        await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(10));
        await _socket.NotififyGroup(workspaceId, "WorkspaceRoleUpdated", dto);
        await _cache.DeleteAsync(CacheKeys.WorkspaceRoles(workspaceId));

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.WorkspaceRole(workspaceId, id);
        var WorkspaceRole = await _cache.GetOrSetAsync(key, () => _workspaceRoleRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("WorkspaceRole", id);

        var success = await _workspaceRoleRepository.DeleteAsync(WorkspaceRole);
        if (success)
        {
            await _socket.NotififyGroup(WorkspaceRole.WorkspaceId, "WorkspaceRoleDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.WorkspaceRoles(workspaceId),
                CacheKeys.WorkspaceRole(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<WorkspaceRoleDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<WorkspaceRole?, WorkspaceRoleDto?>(
        CacheKeys.WorkspaceRole(workspaceId, id),
        () => _workspaceRoleRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<WorkspaceRoleDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<WorkspaceRole>, List<WorkspaceRoleDto>>(
        CacheKeys.WorkspaceRoles(workspaceId),
        () => _workspaceRoleRepository.GetByWorkspaceIdAsync(workspaceId),
        ttl
    );

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
