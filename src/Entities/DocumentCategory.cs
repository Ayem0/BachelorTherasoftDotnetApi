using BachelorTherasoftDotnetApi.src.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class DocumentCategory : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Icon { get; set; }

    public List<Document> Documents { get; set; } = [];
}
