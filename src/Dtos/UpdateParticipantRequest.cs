using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UpdateParticipantRequest
{
    public string? NewParticipantCategoryId { get; set; }
    public string? NewFirstName { get; set; }
    public string? NewLastName { get; set; }
    public string? NewEmail { get; set; }
    public string? NewDescription { get; set; }
    public string? NewAddress{ get; set; }
    public string? NewCity { get; set; }
    public string? NewCountry { get; set; }
    public DateTime? NewDateOfBirth { get; set; }
}
