namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateParticipantRequest
{
    public required string WorkspaceId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string ParticipantCategoryId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
