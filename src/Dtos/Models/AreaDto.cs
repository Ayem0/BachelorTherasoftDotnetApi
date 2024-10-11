using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class AreaDto
{
    public AreaDto(Area area)
    {
        Name = area.Name;
        Description = area.Description;
        Id = area.Id;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
}

public class AreaWithRoomsDto : AreaDto
{
    public AreaWithRoomsDto(Area area) : base(area)
    {
        Rooms = area.Rooms.Select(x => new RoomDto(x)).ToList();
    }
    public List<RoomDto> Rooms { get; set; }
}
