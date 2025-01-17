﻿using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Area : BaseModel, BaseAuthorizationModel
{
    public Area(Location location, string name, string? description)
    {
        Location = location;
        LocationId = location.Id;
        WorkspaceId = location.WorkspaceId;
        Name = name;
        Description = description;
    }
    public Area(string workspaceId, string locationId, string name, string? description)
    {
        WorkspaceId = workspaceId;
        LocationId = locationId;
        Name = name;
        Description = description;
    }
    public string WorkspaceId { get; set; }
    public string LocationId { get; set; }
    public required Location Location { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Room> Rooms { get; set; } = [];
}

