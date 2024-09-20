using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

internal class Comment : DefaultFields
{
    public string Id = Guid.NewGuid().ToString();
    public required string MemberId { get; set; }
    public required string TaskId { get; set; }
    public required string Content { get; set; }
}

