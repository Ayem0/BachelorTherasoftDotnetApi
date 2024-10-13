using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class WorkspaceDetailsDto : WorkspaceDto
{
    public List<LocationDto> Locations { get; set; } = [];
    public List<SlotDto> Slots { get; set; } = [];
    public List<EventCategoryDto> EventCategories { get; set; } = [];
    public List<ParticipantDto> Participants { get; set; } = [];
    public List<ParticipantCategoryDto> ParticipantCategories { get; set; } = [];
    public List<WorkspaceRoleDto> WorkspaceRoles { get; set; } = [];
    public List<TagDto> Tags { get; set; } = [];
    public List<UserDto> Users { get; set; } = [];
}

public class WorkspaceDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
