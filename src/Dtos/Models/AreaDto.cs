using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class AreaDto
{
    public AreaDto(Area area)
    {
        Name = area.Name;
        Description = area.Description;
        Id = area.Id;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}
