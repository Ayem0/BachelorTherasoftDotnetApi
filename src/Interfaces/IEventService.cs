using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IEventService
{
    Task<EventDto?> GetByIdAsync(string id);
    Task<EventDto?> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription, string? newEventCategoryId);
}
