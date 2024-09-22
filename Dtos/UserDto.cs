using System;

namespace BachelorTherasoftDotnetApi.Dtos;

public class UserDto
{
    public required string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
