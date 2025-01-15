using System;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateParticipantRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ParticipantCategoryId { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public string? Address{ get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
