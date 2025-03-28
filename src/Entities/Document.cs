using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Document : BaseEntity
{
    public required string EventId { get; set; }
    public required Event Event { get; set; }
    public required string DocumentCategoryId { get; set; }
    public required DocumentCategory DocumentCategory { get; set; }
}
