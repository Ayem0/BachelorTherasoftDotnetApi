using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Workspace : BaseModel
{
        public Workspace(string name, string? description, List<User> users)
        {
                Name = name;
                Description = description;
                Users = users;
        }
        public Workspace(string name, string? description)
        {
                Name = name;
                Description = description;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<User> Users { get; set; } = [];
        public List<WorkspaceRight> WorkspaceRights { get; set; } = [];
        public List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
        public List<Location> Locations { get; set; } = [];

}
