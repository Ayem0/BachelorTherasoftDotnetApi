using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateLocationRequest
{
    public required string WorkspaceId { get; set; }
    public required string Name { get; set; }
}
