using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IEventService
{
    Task<EventDto?> GetByIdAsync(string id);
    Task<EventDto?> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate, 
        List<string>? participantIds, List<string>? tagIds);
    Task<bool> DeleteAsync(string id);
    Task<EventDto?> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription, 
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? tagIds);
}
