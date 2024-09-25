using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class User_Workspace : BaseModel
{
    public required string UserId { get; set; }
    public required User User { get; set; }
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
}
