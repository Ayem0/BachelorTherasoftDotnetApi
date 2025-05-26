using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;

    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        IUserRepository userRepository,
        IMapper mapper,
        ISocketService socket
    )
    {
        _workspaceRepository = workspaceRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _socket = socket;
    }

    public async Task<WorkspaceDto> CreateAsync(string userId, CreateWorkspaceRequest req)
    {
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("User", userId);
        var workspace = new Workspace(req.Name, req.Description);
        workspace.Users.Add(user);
        var created = await _workspaceRepository.CreateAsync(workspace);
        var dto = _mapper.Map<WorkspaceDto>(workspace);

        await _socket.NotififyGroup(created.Id, "WorkspaceCreated", dto);
        return dto;
    }

    public async Task<WorkspaceDto> UpdateAsync(string id, UpdateWorkspaceRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id) ?? throw new NotFoundException("Workspace", id);

        workspace.Name = req.Name ?? workspace.Name;
        workspace.Description = req.Description ?? workspace.Description;

        var updated = await _workspaceRepository.UpdateAsync(workspace);
        var dto = _mapper.Map<WorkspaceDto>(updated);
        await _socket.NotififyGroup(updated.Id, "WorkspaceUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id) ?? throw new NotFoundException("Workspace", id);
        var success = await _workspaceRepository.DeleteAsync(workspace);
        if (success)
        {
            await _socket.NotififyGroup(id, "WorkspaceDeleted", id);
        }
        return success;
    }

    public async Task<WorkspaceDto?> GetByIdAsync(string id)
    => _mapper.Map<WorkspaceDto?>(await _workspaceRepository.GetByIdAsync(id));

    public async Task<List<WorkspaceDto>> GetByUserIdAsync(string userId)
    => _mapper.Map<List<WorkspaceDto>>(await _workspaceRepository.GetByUserIdAsync(userId));

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
