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
    public string Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
}
