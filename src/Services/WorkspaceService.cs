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

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;
    private readonly IRedisService _cache;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        UserManager<User> userManager,
        IMapper mapper,
        ISocketService socket,
        IRedisService cache
    )
    {
        _workspaceRepository = workspaceRepository;
        _userManager = userManager;
        _mapper = mapper;
        _socket = socket;
        _cache = cache;
    }

    public async Task<WorkspaceDto> CreateAsync(string userId, CreateWorkspaceRequest req)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User", userId);

        var workspace = new Workspace(req.Name, req.Description);
        workspace.Users.Add(user);

        var created = await _workspaceRepository.CreateAsync(workspace);
        var dto = _mapper.Map<WorkspaceDto>(workspace);

        await _socket.NotififyGroup(created.Id, "WorkspaceCreated", dto);
        await _cache.SetAsync(CacheKeys.Workspace(created.Id), created, ttl);
        await _cache.DeleteAsync(CacheKeys.UserWorkspaces(userId));

        return dto;
    }

    public async Task<WorkspaceDto> UpdateAsync(string id, UpdateWorkspaceRequest req)
    {
        var key = CacheKeys.Workspace(id);
        var Workspace = await _cache.GetOrSetAsync(key, () => _workspaceRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Workspace", id);

        Workspace.Name = req.Name ?? Workspace.Name;
        Workspace.Description = req.Description ?? Workspace.Description;

        var updated = await _workspaceRepository.UpdateAsync(Workspace);
        var dto = _mapper.Map<WorkspaceDto>(updated);

        await _cache.SetAsync(key, dto, ttl);
        await _socket.NotififyGroup(updated.Id, "WorkspaceUpdated", dto);
        // TODO remove every key with pattern user:{id}:workspaces foreach workspace user 

        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var key = CacheKeys.Workspace(id);
        var Workspace = await _cache.GetOrSetAsync(key, () => _workspaceRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Workspace", id);

        var success = await _workspaceRepository.DeleteAsync(Workspace);
        if (success)
        {
            await _socket.NotififyGroup(id, "WorkspaceDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.Workspace(id)
            ]);
        }
        return success;
    }

    public Task<WorkspaceDto?> GetByIdAsync(string id)
    => _cache.GetOrSetAsync<Workspace?, WorkspaceDto?>(
        CacheKeys.Workspace(id),
        () => _workspaceRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<WorkspaceDto>> GetByUserIdAsync(string userId)
    => _cache.GetOrSetAsync<List<Workspace>, List<WorkspaceDto>>(
        CacheKeys.UserWorkspaces(userId),
        () => _workspaceRepository.GetByUserIdAsync(userId),
        ttl
    );

    // TODO do this in the user service, and user controller
    // public async Task<List<MemberDto>> GetMembersByIdAsync(string workspaceId)
    // {
    //     var setKey = $"workspace:{workspaceId}:users";
    //     var cachedKeys = await _cache.GetSetAsync(setKey);
    //     if (cachedKeys.Any())
    //     {
    //         var cachedUsers = await _cache.GetHashesAsync<MemberDto>(cachedKeys);
    //         return cachedUsers;
    //     }
    //     var workspace = await _workspaceRepository.GetJoinUsersByIdAsync(workspaceId) ?? throw new NotFoundException("Workspace", workspaceId);
    //     var membersDto = _mapper.Map<List<MemberDto>>(workspace.Users);
    //     await _cache.SetHashesAsync(membersDto.Select(m => $"user:{m.Id}"), membersDto, TimeSpan.FromMinutes(10));
    //     await _cache.AddSetAsync(setKey, membersDto.Select(m => $"user:{m.Id}"), TimeSpan.FromMinutes(5));
    //     return membersDto;
    // }

    // public async Task<ActionResult> RemoveMemberAsync(string id, string userId)
    // {
    //     var workspace = await _workspaceRepository.GetByIdAsync(id);
    //     if (workspace == null) return Response.NotFound(id, "Workspace");

    //     var workspaceUser = workspace.Users.Where(x => x.UserId == userId).First();
    //     if (workspaceUser == null) return Response.NotFound(userId, "User");

    //     workspaceUser.DeletedAt = DateTime.UtcNow;

    //     var res2 = await _workspaceRepository.UpdateAsync(workspace);

    //     return Response.Ok("Successfully removed member.");
    // }



}
