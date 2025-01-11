using System;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;

public class UpdateUserRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
