
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface ILocationRepository : IBaseRepository<Location>
{
    Task<Location?> GetDetailsByIdAsync(string id);
    Task<List<Location>> GetByWorkspaceIdAsync(string id);
    Task<Location?> GetByIdJoinWorkspaceAsync(string id);
}
