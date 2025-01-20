using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ISlotService
{
    Task<SlotDto> GetByIdAsync(string id);
    Task<List<SlotDto>> CreateAsync(CreateSlotRequest request);
    // Task<ActionResult<SlotDto>> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate, TimeOnly? newStartTime, TimeOnly? newEndTime);
    Task<bool> DeleteAsync(string id);
    Task<bool> AddSlotToRoom(string slotId, string roomId);
    Task<List<SlotDto>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request);
    Task<List<SlotDto>> GetByWorkpaceIdAsync(string id);
}
