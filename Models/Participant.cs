using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class Participant : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public List<Event> Events { get; set; } = [];
    public required string ParticipantCategoryId { get; set; }
    public required ParticipantCategory ParticipantCategory { get; set; }
}

