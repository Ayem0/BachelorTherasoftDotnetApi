using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Workspace : BaseModel
{
        public Workspace(string name, string? description, List<WorkspaceUser> users)
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
        public List<WorkspaceUser> Users { get; set; } = [];
        public List<Slot> Slots { get; set; } = [];
        public List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
        public List<Location> Locations { get; set; } = [];
        public List<EventCategory> EventCategories { get; set; } = [];
        public List<ParticipantCategory> ParticipantCategories { get; set; } = [];
        public List<Participant> Participants { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
}
