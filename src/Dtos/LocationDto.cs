namespace BachelorTherasoftDotnetApi.src.Dtos;

public class LocationDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<AreaDto>? Areas { get; set; }
}
