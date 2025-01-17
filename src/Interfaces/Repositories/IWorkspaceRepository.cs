using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IWorkspaceRepository
{
    Task<Workspace> CreateAsync(Workspace workspace);
    Task<Workspace?> GetByIdAsync(string id);
    Task<Workspace> UpdateAsync(Workspace workspace);
    Task<bool> DeleteAsync(string id);
    Task<Workspace?> GetDetailsByIdAsync(string id);
    Task<Workspace[]> GetByUserIdAsync(string id);
    Task<Workspace?> GetJoinUsersByIdAsync(string id);
}
