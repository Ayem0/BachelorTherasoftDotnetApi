using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class TagDto
{
    public TagDto(Tag tag)
    {
        Id = tag.Id;
        Name = tag.Name;
        Icon = tag.Icon;
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
}
