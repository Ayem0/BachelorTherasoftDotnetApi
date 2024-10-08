using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class ConversationDto
{
    public ConversationDto(Conversation conversation)
    {
        Id = conversation.Id;
        Name = conversation.Name;
        UserIds = conversation.UserIds;
    }
    public string Id { get; set;}
    public string? Name { get; set;}
    public List<string> UserIds { get; set;}
}
