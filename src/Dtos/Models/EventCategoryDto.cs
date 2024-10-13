using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class EventCategoryDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Icon { get; set; } = string.Empty;
}
