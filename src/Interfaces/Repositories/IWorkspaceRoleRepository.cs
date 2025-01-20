using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IWorkspaceRoleRepository : IBaseRepository<WorkspaceRole>
{
    Task<List<WorkspaceRole>> GetByWorkspaceIdAsync(string id);
}
