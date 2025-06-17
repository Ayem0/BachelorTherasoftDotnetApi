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
    private readonly ILogger<WorkspaceService> _logger;

    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        IUserRepository userRepository,
        IMapper mapper,
        ISocketService socket,
        ILogger<WorkspaceService> logger
    )
    {
        _workspaceRepository = workspaceRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _socket = socket;
        _logger = logger;
    }

    public async Task<WorkspaceDto?> GetByIdAsync(string id)
    => _mapper.Map<WorkspaceDto?>(await _workspaceRepository.GetByIdAsync(id));

    public async Task<List<WorkspaceDto>> GetByUserIdAsync(string userId)
    => _mapper.Map<List<WorkspaceDto>>(await _workspaceRepository.GetByUserIdAsync(userId));

    public async Task<WorkspaceDto> CreateAsync(string userId, CreateWorkspaceRequest req)
    {
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("User", userId);
        var workspace = new Workspace(req.Name, req.Color, req.Description);
        workspace.Users.Add(user);
        var created = await _workspaceRepository.CreateAsync(workspace);
        var dto = _mapper.Map<WorkspaceDto>(created);

        await _socket.NotififyUser(user.Id, "WorkspaceAdded", dto);
        return dto;
    }

    public async Task<WorkspaceDto> UpdateAsync(string id, UpdateWorkspaceRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id) ?? throw new NotFoundException("Workspace", id);

        workspace.Name = req.Name ?? workspace.Name;
        workspace.Color = req.Color ?? workspace.Color;
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

}
