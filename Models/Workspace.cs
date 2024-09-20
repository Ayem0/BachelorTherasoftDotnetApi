using System;
using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class Workspace : DefaultFields
{
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public List<User> Users { get; set; } = [];
        public List<WorkspaceRight> WorkspaceRights { get; set; } = [];
        public List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
        public List<Location> Locations { get; set; } = [];

}
