using System;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class DocumentCategory : BaseEntity
{
    public required string Name { get; set; }
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Icon { get; set; }

    public List<Document> Documents { get; set; } = [];
}
