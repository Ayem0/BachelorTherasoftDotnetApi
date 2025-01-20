using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;
// TODO voir si possible de modeliser mieux les créneaux + voir comment en faire les horaires d'ouvertures maybe champs eventCategories a null mais en vrai un bool c mieux
public class Slot : BaseModel, BaseAuthorizationModel
{
    public Slot(string name, string? description, Workspace workspace, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, List<EventCategory> eventCategories, Interval? repetitionInterval, 
        int? repetitionNumber, Slot? mainSlot, DateOnly? repetitionEndDate)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        StartTime = startTime;
        EndTime = endTime;
        EventCategories = eventCategories;
        RepetitionInterval = repetitionInterval;
        RepetitionNumber = repetitionNumber;
        MainSlot = mainSlot;
        MainSlotId = mainSlot?.Id;
        RepetitionEndDate = repetitionEndDate;
    }

    public Slot(string name, string? description, string workspaceId, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, Interval? repetitionInterval, 
    int? repetitionNumber, string? mainSlotId, DateOnly? repetitionEndDate)
    {
        Name = name;
        Description = description;
        WorkspaceId = workspaceId;
        StartDate = startDate;
        EndDate = endDate;
        StartTime = startTime;
        EndTime = endTime;
        RepetitionInterval = repetitionInterval;
        RepetitionNumber = repetitionNumber;
        MainSlotId = mainSlotId;
        RepetitionEndDate = repetitionEndDate;
    }
    public string Name { get; set; }
    public string? Description { get; set; }
    public required Workspace Workspace { get; set; }
    public string WorkspaceId { get; set; }
    public List<Room> Rooms { get; set; } = [];
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public List<EventCategory>? EventCategories { get; set; }
    public Interval? RepetitionInterval { get; set; }
    public int? RepetitionNumber { get; set; }
    public Slot? MainSlot { get; set; }
    public string? MainSlotId { get; set; }
    public DateOnly? RepetitionEndDate { get; set; }
}
