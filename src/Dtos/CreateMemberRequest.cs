using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateMemberRequest
{
    public required string UserId { get; set; }
    public required string WorkspaceId { get; set;}
}
