using System;
using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class Workspace : BaseModel
{
        public required string Name { get; set; }
        public virtual List<User> Users { get; set; } = [];
        public virtual List<WorkspaceRight> WorkspaceRights { get; set; } = [];
        public virtual List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
        public virtual List<Location> Locations { get; set; } = [];

}
