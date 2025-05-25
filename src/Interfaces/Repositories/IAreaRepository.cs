using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IAreaRepository : IBaseRepository<Area>
{
    Task<List<Area>> GetAreasByLocationIdAsync(string id);
    Task<List<Area>> GetAreasByWorkspaceIdAsync(string id);
    Task<Area?> GetByIdJoinWorkspaceAsync(string id);
}
