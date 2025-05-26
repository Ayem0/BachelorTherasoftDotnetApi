using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ISlotService
{
    public Task<SlotDto?> GetByIdAsync(string id);
    public Task<SlotDto> CreateAsync(CreateSlotRequest request);
    public Task<List<SlotDto>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request);
    // Task<ActionResult<SlotDto>> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate, TimeOnly? newStartTime, TimeOnly? newEndTime);
    public Task<bool> DeleteAsync(string id);
    public Task<List<SlotDto>> GetByWorkspaceIdAsync(string id);
}
