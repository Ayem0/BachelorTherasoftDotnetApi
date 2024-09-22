using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class Message : BaseModel
{
    public required string MemberId { get; set; }
    public required string ConversationId { get; set; }
    public string ?ReplyToMessageId { get; set; }
    public required string Content { get; set; }
    public List<string> ?ReplysMessageId { get; set; }
}
