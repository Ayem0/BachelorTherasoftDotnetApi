using System;

namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateConversationRequest
{
    public string? Name { get; set; }
    public required List<string> UserIds { get; set; }
}
