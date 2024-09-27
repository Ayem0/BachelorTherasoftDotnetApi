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
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
