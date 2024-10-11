using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ISlotService
{
    Task<ActionResult<SlotDto>> GetByIdAsync(string id);
    Task<ActionResult<SlotDto>> CreateAsync(CreateSlotRequest request);
    // Task<ActionResult<SlotDto>> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate, TimeOnly? newStartTime, TimeOnly? newEndTime);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult> AddSlotToRoom(string slotId, string roomId);
    Task<ActionResult<List<SlotDto>>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request);
}
