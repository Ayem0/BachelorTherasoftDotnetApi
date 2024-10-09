using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ISlotService
{
    Task<Response<SlotDto?>> GetByIdAsync(string id);
    Task<Response<SlotDto?>> CreateAsync(CreateSlotRequest request);
    // Task<Response<SlotDto?>> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate, TimeOnly? newStartTime, TimeOnly? newEndTime);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<string>> AddSlotToRoom(string slotId, string roomId);
    Task<Response<List<SlotDto>?>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request);
}
