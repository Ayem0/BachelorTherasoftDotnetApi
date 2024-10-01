using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateSlotRequest
{
    public required string WorkspaceId { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
}
