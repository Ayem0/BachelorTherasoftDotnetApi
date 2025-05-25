using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ISlotService
{
    public Task<SlotDto?> GetByIdAsync(string workspaceId, string id);
    public Task<SlotDto> CreateAsync(string workspaceId, CreateSlotRequest request);
    public Task<List<SlotDto>> CreateWithRepetitionAsync(string workspaceId, CreateSlotWithRepetitionRequest request);
    // Task<ActionResult<SlotDto>> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate, TimeOnly? newStartTime, TimeOnly? newEndTime);
    public Task<bool> DeleteAsync(string workspaceId, string id);
    public Task<List<SlotDto>> GetByWorkspaceIdAsync(string id);
}
