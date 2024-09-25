using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Slot : BaseModel
{
    public required string RoomId { get; set; }
    public required Room Room { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public List<DayOfWeek> ?Days { get; set; } // Voir si moyen de faire autre chose / a modifier dans le dbcontext pour en faire un objet json
    public List<EventCategory> EventCategories { get; set; } = [];
    public int ?IntervalDelay { get; set; }
    public Interval ?Interval { get; set; } // string a changer par un enum dayly weekly monthly annual / a modifier dans le dbcontext en json
}
