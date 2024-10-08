using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ISlotService
{
    Task<SlotDto?> GetByIdAsync(string id);
    Task<SlotDto?> CreateAsync(string workspaceId, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, List<string>? eventCategoryIds);
    Task<SlotDto?> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate, TimeOnly? newStartTime, TimeOnly? newEndTime);
    Task<bool> DeleteAsync(string id);
    Task<bool> AddSlotToRoom(string slotId, string roomId);
    Task<List<SlotDto>?> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request);
}
