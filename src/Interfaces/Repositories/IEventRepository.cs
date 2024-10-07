using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event?> GetByIdWithRelationsAsync(string id);
    Task<Event?> GetByIdJoinWorkspaceAsync(string id);
}
