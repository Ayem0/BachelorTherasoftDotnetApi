using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class SlotDto
{
    public SlotDto(Slot slot)
    {
        WorkspaceId = slot.WorkspaceId;
        StartDate = slot.StartDate;
        EndDate = slot.EndDate;
    }
    public string WorkspaceId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
