using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventService
{
    Task<Response<EventDto?>> GetByIdAsync(string id);
    Task<Response<EventDto?>> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate,
        List<string>? participantIds, List<string>? tagIds);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<EventDto?>> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription,
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? tagIds);
    Task<Response<List<EventDto>?>> CreateWithRepetitionAsync(CreateEventWithRepetitionRequest request);
}
