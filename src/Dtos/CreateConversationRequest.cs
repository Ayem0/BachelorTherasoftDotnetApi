using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateConversationRequest
{
    public string? Name { get; set; }
    public required List<string> UserIds { get; set; }
}
