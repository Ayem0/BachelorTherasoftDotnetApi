using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class ParticipantDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public required string ParticipantCategoryId { get; set; }
    [Required]
    public required string WorkspaceId { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class ParticipantJoinCategoryDto : ParticipantDto
{
    public required ParticipantCategoryDto ParticipantCategory { get; set; }
}
