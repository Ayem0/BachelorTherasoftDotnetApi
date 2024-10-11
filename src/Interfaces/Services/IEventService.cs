using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventService
{
    Task<ActionResult<EventDto>> GetByIdAsync(string id);
    Task<ActionResult<EventDto>> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate,
        List<string>? participantIds, List<string>? tagIds);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<EventDto>> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription,
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? tagIds);
    Task<ActionResult<List<EventDto>>> CreateWithRepetitionAsync(CreateEventWithRepetitionRequest request);
}
