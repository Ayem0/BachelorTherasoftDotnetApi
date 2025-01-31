using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
    Task<Event?> GetByIdJoinRelationsAsync(string id);
    Task<Event?> GetByIdJoinWorkspaceAsync(string id);
    Task<List<Event>> GetByRangeAndUserIdAsync(string id, DateTime start, DateTime end);
    Task<List<Event>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end);
}
