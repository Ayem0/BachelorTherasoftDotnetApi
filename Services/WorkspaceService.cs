using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    public WorkspaceService(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }
    public async Task<Workspace?> GetWorkspaceByIdAsync(string id)
    {
        return await _workspaceRepository.GetByIdAsync(id);
    }

    public async Task CreateWorkspaceAsync(Workspace workspace) 
    {
        await _workspaceRepository.CreateAsync(workspace);
    }

    public async Task UpdateWorkspaceAsync(Workspace workspace)
    {
        await _workspaceRepository.UpdateAsync(workspace);
    }

    public async Task DeleteWorkspaceAsync(string id)
    {
        await _workspaceRepository.DeleteAsync(id);
    }
}
