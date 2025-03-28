using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IWorkspaceRepository : IBaseRepository<Workspace>
{
    Task<Workspace?> GetDetailsByIdAsync(string id);
    Task<Workspace[]> GetByUserIdAsync(string id);
    Task<Workspace?> GetJoinUsersByIdAsync(string id);
}
