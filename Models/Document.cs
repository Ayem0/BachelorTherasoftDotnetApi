using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class Document : BaseModel
{
    public required string EventId { get; set; }
    public required Event Event { get; set; }
    public required string DocumentCategoryId { get; set; }
    public required DocumentCategory DocumentCategory { get; set; }
}
