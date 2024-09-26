namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateAreaRequest
{
    public required string Name { get; set; }
    public required string LocationId { get; set; }
    public string? Description { get; set; }
}
