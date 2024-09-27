using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class AreaDto
{
    public AreaDto(Area area)
    {
        Name = area.Name;
        Description = area.Description;
        Id = area.Id;
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
