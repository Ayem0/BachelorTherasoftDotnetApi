using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventService
{
    Task<EventDto> GetByIdAsync(string id);
    Task<EventDto?> CreateAsync(string userId, CreateEventRequest req);
    Task<bool> DeleteAsync(string id);
    Task<EventDto> UpdateAsync(string id, UpdateEventRequest req);
    Task<List<EventDto>?> CreateWithRepetitionAsync(CreateEventWithRepetitionRequest request);
    Task<List<EventDto>> GetByRangeAndUserIdAsync(string id, DateTime start, DateTime end);
    Task<List<EventDto>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end);
}
