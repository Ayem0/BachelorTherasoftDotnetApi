namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateRoomRequest
{
    public required string Name { get; set; }
    public required string AreaId { get; set; }
    public string? Description { get; set; }
}
