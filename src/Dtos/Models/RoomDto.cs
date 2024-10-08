using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class RoomDto
{
    public RoomDto(Room room)
    {
        Id = room.Id;
        Name = room.Name;
        Description = room.Description;
        Slots = room.Slots.Select(x => new SlotDto(x)).ToList();
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    List<SlotDto> Slots{ get; set; }
}
