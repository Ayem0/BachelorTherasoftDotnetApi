using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;


public interface IRoomRepository : IBaseRepository<Room>
{
    Task<Room?> GetJoinEventsSlotsByRangeAndIdAsync(string id, DateTime start, DateTime end);
    Task<List<Room>> GetByAreaIdAsync(string id);
    Task<List<Room>> GetByWorkspaceIdAsync(string id);
}

