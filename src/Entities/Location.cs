using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Location : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public Location() { }
    public Location(Workspace workspace, string name, string? description, string? address, string? city, string? country)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Description = description;
        Address = address;
        City = city;
        Country = country;
    }
    public Location(string workspaceId, string name, string? description, string? address, string? city, string? country)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Description = description;
        Address = address;
        City = city;
        Country = country;
    }
    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public List<Area> Areas { get; set; } = [];
}

