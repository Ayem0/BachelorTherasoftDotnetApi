using System.ComponentModel.DataAnnotations;
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
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Icon { get; set; }
}
