using System;
using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class DocumentCategory : BaseModel
{
    public required string Name { get; set; }
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Icon { get; set; }

    public List<Document> Documents { get; set; } = [];
}
