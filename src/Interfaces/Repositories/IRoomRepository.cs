using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;


public interface IRoomRepository
{
    Task<Room> CreateAsync(Room room);
    Task<Room?> GetByIdAsync(string id);
    Task<Room> UpdateAsync(Room room);
    Task<bool> DeleteAsync(string id);
    Task<Room?> GetDetailsByIdAsync(string id);
}

