using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Workspace : BaseModel
{
        public required string Name { get; set; }
        public string? Description { get; set; }
        public virtual List<User> Users { get; set; } = [];
        public virtual List<WorkspaceRight> WorkspaceRights { get; set; } = [];
        public virtual List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
        public virtual List<Location> Locations { get; set; } = [];

}
