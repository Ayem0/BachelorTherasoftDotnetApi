using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class EventDto
{
    public EventDto(Event baseEvent, RoomDto roomDto, EventCategoryDto eventCategoryDto, List<ParticipantDto>? participantDtos, List<TagDto>? tagDtos)
    {
        Id = baseEvent.Id;
        StartDate = baseEvent.StartDate;
        EndDate = baseEvent.EndDate;
        Description = baseEvent.Description;
        Room = roomDto;
        EventCategory = eventCategoryDto;
        Participants = participantDtos;
        Tags = tagDtos;
    }
    public string Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public RoomDto Room { get; set; }
    public EventCategoryDto EventCategory { get; set; }
    public List<ParticipantDto>? Participants { get; set; }
    public List<TagDto>? Tags { get; set; }
}
