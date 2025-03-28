using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Conversation : BaseEntity
{
    public Conversation(List<string> userIds, string? name)
    {
        UserIds = userIds;
        Name = name;
    }
    public string? Name { get; set; }
    public List<string> UserIds { get; set; }
    public List<Message> Messages { get; set; } = [];
}

