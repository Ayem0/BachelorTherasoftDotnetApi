using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IWorkspaceRepository
{
    Task CreateAsync(string name, User user);
    Task UpdateAsync(string workspaceId, string name);
    Task AddUserAsync(string workspaceId, User user);
    Task RemoveUserAsync(string workspaceId, User user);
    Task DeleteAsync(string workspaceId);

    Task<Workspace?> GetWorkspaceAsync(string workspaceId);
    Task<List<Workspace>?> GetWorkspacesAsync(string[] workspaceIds);

}
