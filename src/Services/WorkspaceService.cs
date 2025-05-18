using System.Text.Json;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IHubService _hubService;
    private readonly IRedisService _redisService;
    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        UserManager<User> userManager,
        IMapper mapper,
        IHubService hubService,
        IRedisService redisService
    )
    {
        _workspaceRepository = workspaceRepository;
        _userManager = userManager;
        _mapper = mapper;
        _hubService = hubService;
        _redisService = redisService;
    }

    public async Task<WorkspaceDto> GetByIdAsync(string id)
    {
        var cacheKey = $"workspace:{id}";
        var cacheValue = await _redisService.GetHashAsync<WorkspaceDto>(cacheKey);
        if (cacheValue != null)
            return cacheValue;

        var workspace = await _workspaceRepository.GetByIdAsync(id) ?? throw new NotFoundException("Workspace", id);
        var workspaceDto = _mapper.Map<WorkspaceDto>(workspace);

        await _redisService.SetHashAsync(cacheKey, workspaceDto, TimeSpan.FromMinutes(10));
        return workspaceDto;
    }

    public async Task<List<WorkspaceDto>> GetByUserIdAsync(string id)
    {
        var setKey = $"user:{id}:workspaces";
        var cachedKeys = await _redisService.GetSetAsync(setKey);
        if (cachedKeys.Any())
        {
            var cachedWorkspaces = await _redisService.GetHashesAsync<WorkspaceDto>(cachedKeys);
            return cachedWorkspaces;
        }
        var workspaces = await _workspaceRepository.GetByUserIdAsync(id) ?? throw new NotFoundException("Workspace", id);
        var workspacesDto = _mapper.Map<List<WorkspaceDto>>(workspaces);
        await _redisService.SetHashesAsync(workspacesDto.Select(w => $"workspace:{w.Id}"), workspacesDto, TimeSpan.FromMinutes(10));
        await _redisService.AddSetAsync(setKey, workspacesDto.Select(w => $"workspace:{w.Id}"), TimeSpan.FromMinutes(5));

        return workspacesDto;
    }

    public async Task<WorkspaceDto> CreateAsync(string userId, CreateWorkspaceRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User", userId);

        var workspace = new Workspace(request.Name, request.Description);

        workspace.Users.Add(user);

        var createdWorkspace = await _workspaceRepository.CreateAsync(workspace);
        var workspaceDto = _mapper.Map<WorkspaceDetailsDto>(createdWorkspace);

        await _redisService.AddToSetAsync($"user:{userId}:workspaces", workspaceDto.Id);
        await _redisService.SetHashAsync($"workspace:{workspaceDto.Id}", workspaceDto, TimeSpan.FromMinutes(10));
        await _hubService.NotififyUser(userId, "WorkspaceCreated", workspaceDto);

        return workspaceDto;
    }

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

    public async Task<bool> DeleteAsync(string id)
    {
        return await _workspaceRepository.DeleteAsync(id);
    }

    public async Task<WorkspaceDto> UpdateAsync(string id, UpdateWorkspaceRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id) ?? throw new NotFoundException("Workspace", id);

        workspace.Name = req.Name ?? workspace.Name;
        workspace.Description = req.Description ?? workspace.Description;

        await _workspaceRepository.UpdateAsync(workspace);
        var workspaceDto = _mapper.Map<WorkspaceDetailsDto>(workspace);

        await _hubService.NotififyGroup(workspace.Id, "WorkspaceUpdated", workspaceDto);
        await _redisService.SetHashAsync($"workspace:{workspace.Id}", workspaceDto, TimeSpan.FromMinutes(10));

        return workspaceDto;
    }

    public async Task<List<MemberDto>> GetMembersByIdAsync(string workspaceId)
    {
        var setKey = $"workspace:{workspaceId}:users";
        var cachedKeys = await _redisService.GetSetAsync(setKey);
        if (cachedKeys.Any())
        {
            var cachedUsers = await _redisService.GetHashesAsync<MemberDto>(cachedKeys);
            return cachedUsers;
        }
        var workspace = await _workspaceRepository.GetJoinUsersByIdAsync(workspaceId) ?? throw new NotFoundException("Workspace", workspaceId);
        var membersDto = _mapper.Map<List<MemberDto>>(workspace.Users);
        await _redisService.SetHashesAsync(membersDto.Select(m => $"user:{m.Id}"), membersDto, TimeSpan.FromMinutes(10));
        await _redisService.AddSetAsync(setKey, membersDto.Select(m => $"user:{m.Id}"), TimeSpan.FromMinutes(5));
        return membersDto;
    }
}
