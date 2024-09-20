using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class Document : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string EventId { get; set; }
    public required Event Event { get; set; }
    public required string DocumentCategoryId { get; set; }
    public required DocumentCategory DocumentCategory { get; set; }
}
