using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IRoomRepository
{
    Task CreateRoomAsync(string areaId, string name);
    Task<Room?> GetRoomAsync(string roomId);
    Task<List<Room>?> GetRoomsAsync(string[] roomIds);
    Task UpdateRoomAsync(string roomId, string? name, string? areaId);
    Task DeleteRoomAsync(string roomId);
}
