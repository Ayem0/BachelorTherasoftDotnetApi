using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IWorkspaceRepository : IBaseRepository<Workspace>
{
    Task<List<Workspace>> GetByUserIdAsync(string id);
}
