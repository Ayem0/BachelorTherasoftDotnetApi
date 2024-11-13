using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class AreaDto
{
    [Required]
    public required string Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<RoomDto> Rooms { get; set; } = [];
}