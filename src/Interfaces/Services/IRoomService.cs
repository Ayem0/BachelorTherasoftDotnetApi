
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRoomService
{
    Task<RoomDto?> GetByIdAsync(string id);
    Task<RoomDto?> CreateAsync(string name, string areaId, string? Description);
    Task<bool> DeleteAsync(string id);
    Task<RoomDto?> UpdateAsync(string id, string? newName, string? newDescription);
}
