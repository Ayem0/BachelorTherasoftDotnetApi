using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Document : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public required string EventId { get; set; }
    public required Event Event { get; set; }
    public required string DocumentCategoryId { get; set; }
    public required DocumentCategory DocumentCategory { get; set; }
}
