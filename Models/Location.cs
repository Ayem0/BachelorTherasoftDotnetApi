using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class Location : BaseModel
{
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Name { get; set; }
    public string ?Address { get; set; }
    public string ?City { get; set; }
    public string ?Country { get; set; }

    public List<Area> Areas { get; set; } = [];
}

