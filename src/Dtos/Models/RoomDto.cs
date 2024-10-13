using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class RoomDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class RoomWithSlotsDto : RoomDto
{
    List<SlotDto> Slots{ get; set; } = [];
}
