using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

internal class Message : DefaultFields
{
    public string Id = Guid.NewGuid().ToString();
    public required string MemberId { get; set; }
    public required string ConversationId { get; set; }
    public string ?ReplyToMessageId { get; set; }
    public required string Content { get; set; }
    public List<string> ?ReplysMessageId { get; set; }
}
