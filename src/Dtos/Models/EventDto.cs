using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

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
    [Required]
    public string Id { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    [Required]
    public RoomDto Room { get; set; }
    [Required]
    public EventCategoryDto EventCategory { get; set; }
    public List<ParticipantDto>? Participants { get; set; }
    public List<TagDto>? Tags { get; set; }
}
