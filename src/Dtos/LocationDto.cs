using System;
using System.Text.Json.Serialization;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class LocationDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<AreaDto>? Areas { get; set; }
}
