using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UpdateLocationRequest
{
    public required string LocationId { get; set; }
    public required string NewName { get; set; }
}
