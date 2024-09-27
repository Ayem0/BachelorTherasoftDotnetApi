using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Slot : BaseModel
{
    public Slot(Room room, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
    {
        Room = room;
        RoomId = room.Id;
        StartTime = startTime;
        EndTime = endTime;
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public Slot(string roomId, TimeOnly startTime, TimeOnly endTime, DateOnly startDate, DateOnly endDate)
    {
        RoomId = roomId;
        StartTime = startTime;
        EndTime = endTime;
        StartDate = startDate;
        EndDate = endDate;
    }

    public string RoomId { get; set; }
    public required Room Room { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<DayOfWeek> ?Days { get; set; } // Voir si moyen de faire autre chose / a modifier dans le dbcontext pour en faire un objet json
    public List<EventCategory> EventCategories { get; set; } = [];
    public int ?IntervalDelay { get; set; }
    public Interval ?Interval { get; set; } // string a changer par un enum dayly weekly monthly annual / a modifier dans le dbcontext en json
}
