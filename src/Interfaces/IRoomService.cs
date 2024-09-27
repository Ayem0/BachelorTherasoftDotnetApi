
using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IRoomService
{
    Task<RoomDto?> GetByIdAsync(string id);
    Task<RoomDto?> CreateAsync(string name, string areaId, string? Description);
    Task<bool> DeleteAsync(string id);
    Task<RoomDto?> UpdateAsync(string id, string? newName, string? newDescription);
}
