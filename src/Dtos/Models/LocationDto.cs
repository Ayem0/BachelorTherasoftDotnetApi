using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class LocationDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<AreaDto>? Areas { get; set; }
}
