using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class RoomDto
{
    public RoomDto(Room room)
    {
        Id = room.Id;
        Name = room.Name;
        Description = room.Description;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}
