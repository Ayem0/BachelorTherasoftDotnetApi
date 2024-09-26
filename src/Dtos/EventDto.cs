namespace BachelorTherasoftDotnetApi.src.Dtos;

public class EventDto
{
    public required string Id { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public required RoomDto Room { get; set; }
    public required EventCategoryDto EventCategory { get; set; }
}
