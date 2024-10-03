using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;
// TODO voir si possible de modeliser mieux les créneaux + voir comment en faire les horaires d'ouvertures maybe champs eventCategories a null mais en vrai un bool c mieux
public class Slot : BaseModel
{
    public Slot(Workspace workspace, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, List<EventCategory> eventCategories)
    {
        // EventCategories = eventCategories;
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        // Rooms = rooms;
        // StartTime = startTime;
        // EndTime = endTime;
        StartDate = startDate;
        EndDate = endDate;
        StartTime = startTime;
        EndTime = endTime;
        EventCategories = eventCategories;
    }


    public Slot(string workspaceId, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime)
    {
        WorkspaceId = workspaceId;
        StartDate = startDate;
        EndDate = endDate;
        StartTime = startTime;
        EndTime = endTime;

    }
    public required Workspace Workspace { get; set; }
    public string WorkspaceId { get; set; }


    public List<Room> Rooms { get; set; } = [];
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public List<DayOfWeek>? Days { get; set; } // Voir si moyen de faire autre chose / a modifier dans le dbcontext pour en faire un objet json
    public List<EventCategory> EventCategories { get; set; } = [];
    public int? IntervalDelay { get; set; }
    public Interval? Interval { get; set; } // string a changer par un enum dayly weekly monthly annual / a modifier dans le dbcontext en json
}
