using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class EventDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
}

public class EventWithRoomDto : EventDto
{
    [Required]
    public RoomDto Room { get; set; } = new();
}

public class EventWithEventCategoryDto : EventDto
{    
    public EventCategoryDto EventCategory { get; set; } = new();
}

public class EventWithParticipantsDto : EventDto
{    
    public List<ParticipantDto> Participants { get; set; } = [];
}

public class EventWithTagsDto : EventDto
{    
    public List<TagDto> Tags { get; set; } = [];
}

public class EventDetailsDto : EventDto 
{    
    public List<TagDto> Tags { get; set; } = [];
    public List<ParticipantDto> Participants { get; set; } = [];
    public EventCategoryDto EventCategory { get; set; } = new();
    [Required]
    public RoomDto Room { get; set; } = new();
}