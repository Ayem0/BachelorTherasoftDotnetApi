using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class ParticipantCategoryDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Color { get; set; } = string.Empty;
    [Required]
    public string Icon { get; set; } = string.Empty;
    public string? Description { get; set; }
}
