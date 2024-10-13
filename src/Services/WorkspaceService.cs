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

    public async Task<ActionResult<WorkspaceDto>> GetByIdAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync<WorkspaceDto>(id);
        if (workspace == null) return Response.NotFound(id, "Workspace");

        return Response.Ok(workspace);
    }

    public async Task<ActionResult<WorkspaceDetailsDto>> GetDetailsByIdAsync(string id)
    {
        var workspace = await _workspaceRepository.GetByIdAsync<WorkspaceDetailsDto>(id);
        if (workspace == null) return Response.NotFound(id, "Workspace");

        return Response.Ok(workspace);
    }

    public async Task<ActionResult<WorkspaceDto>> CreateAsync(string userId, CreateWorkspaceRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Response.NotFound(userId, "User");

        var workspace = new Workspace(request.Name, request.Description);
        var workspaceUser = new WorkspaceUser(user, workspace) { User = user, Workspace = workspace };

        workspace.Users.Add(workspaceUser);

        var res = await _workspaceRepository.CreateAsync(workspace);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);

        return Response.CreatedAt(_mapper.Map<WorkspaceDto>(workspace));
    }

    public async Task<ActionResult> RemoveMemberAsync(string id, string userId)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, "Workspace");

        var workspaceUser = res.Data.Users.Where(x => x.UserId == userId).First();
        if (workspaceUser == null) return Response.NotFound(userId, "User");

        workspaceUser.DeletedAt = DateTime.Now;

        var res2 = await _workspaceRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.Ok("Successfully removed member.");
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _workspaceRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        
        return Response.NoContent();
    }

    public async Task<ActionResult<WorkspaceDto>> UpdateAsync(string id, UpdateWorkspaceRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return new BadRequestObjectResult("At least one field is required.");
        
        var res = await _workspaceRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null ) return new NotFoundObjectResult("Workspace not found.");

        res.Data.Name = request.NewName ?? res.Data.Name;
        res.Data.Description = request.NewDescription ?? res.Data.Description;

        var res2 = await _workspaceRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.Ok(_mapper.Map<WorkspaceDto>(res.Data));
    }
}
