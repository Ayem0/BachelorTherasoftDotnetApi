using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    public WorkspaceService(IWorkspaceRepository workspaceRepository, UserManager<User> userManager, IMapper mapper)
    {
        _workspaceRepository = workspaceRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<WorkspaceDto> GetByIdAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(id) ?? throw new NotFoundException("Workspace", id);

        return _mapper.Map<WorkspaceDto>(workspace);
    }

    public async Task<List<WorkspaceDto>> GetByUserIdAsync(string id)
    {
        var workspaces = await _workspaceRepository.GetByUserIdAsync(id) ?? throw new NotFoundException("Workspace", id);

        return _mapper.Map<List<WorkspaceDto>>(workspaces);
    }

    public async Task<WorkspaceDto> CreateAsync(string userId, CreateWorkspaceRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User", userId);

        var workspace = new Workspace(request.Name, request.Description);

        workspace.Users.Add(user);

        var createdWorkspace = await _workspaceRepository.CreateAsync(workspace);

        return _mapper.Map<WorkspaceDetailsDto>(createdWorkspace);
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

        return _mapper.Map<WorkspaceDto>(workspace);
    }

    public async Task<List<MemberDto>> GetMembersByIdAsync(string workspaceId)
    {
        var workspace = await _workspaceRepository.GetJoinUsersByIdAsync(workspaceId) ?? throw new NotFoundException("Workspace", workspaceId);
        var members = workspace.Users;
        return [.. members.Select(x => new MemberDto { Id = x.Id, FirstName = x.FirstName ?? "", LastName = x.LastName ?? "", WorkspaceId = workspaceId })];

    }
}
