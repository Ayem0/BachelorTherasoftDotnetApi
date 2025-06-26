using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Workspace : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    // Empty ctor for deserialization
    public Workspace() { }
    public Workspace(string name, string color, string? description, List<User> users)
    {
        Name = name;
        Color = color;
        Description = description;
        Users = users;
    }
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Description { get; set; }
    public List<User> Users { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];
    public List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
    public List<Location> Locations { get; set; } = [];
    public List<EventCategory> EventCategories { get; set; } = [];
    public List<ParticipantCategory> ParticipantCategories { get; set; } = [];
    public List<Participant> Participants { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
    public List<Room> Rooms { get; set; } = [];
    public List<Area> Areas { get; set; } = [];
    public List<Event> Events { get; set; } = [];
}
