using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class EventCategoryDto
{
    public EventCategoryDto(EventCategory eventCategory)
    {
        Id = eventCategory.Id;
        Name = eventCategory.Name;
        Icon = eventCategory.Icon;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Icon { get; set; }
}
