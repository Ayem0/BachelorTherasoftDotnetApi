using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class LocationDto
{
    public LocationDto(Location location)
    {
        Id = location.Id;
        Name = location.Name;
        Description = location.Description;
        Address = location.Address;
        City = location.City;
        Country = location.Country;
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<AreaDto>? Areas { get; set; }
}
