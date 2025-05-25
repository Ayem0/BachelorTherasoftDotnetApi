using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Participant : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public Participant() { }
    public Participant(Workspace workspace, ParticipantCategory participantCategory, string firstName, string lastName, string? description, string? email, string? phoneNumber,
        string? address, string? city, string? country, DateTime? dateOfBirth)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        ParticipantCategory = participantCategory;
        ParticipantCategoryId = participantCategory.Id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        City = city;
        Country = country;
        DateOfBirth = dateOfBirth;
        Description = description;
    }

    public Participant(string workspaceId, string participantCategoryId, string firstName, string lastName, string? description, string? email, string? phoneNumber,
        string? address, string? city, string? country, DateTime? dateOfBirth)
    {
        WorkspaceId = workspaceId;
        ParticipantCategoryId = participantCategoryId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        City = city;
        Country = country;
        DateOfBirth = dateOfBirth;
        Description = description;
    }

    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string ParticipantCategoryId { get; set; }
    public required ParticipantCategory ParticipantCategory { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public List<Event> Events { get; set; } = [];
}

