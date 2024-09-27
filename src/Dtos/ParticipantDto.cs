using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class ParticipantDto
{
    public ParticipantDto(Participant participant)
    {
        Id = participant.Id;
        FirstName = participant.FirstName;
        LastName = participant.LastName;
        Email = participant.Email;
        Address = participant.Address;
        City = participant.City;
        Country = participant.Country;
        Description = participant.Description;
        DateOfBirth = participant.DateOfBirth;
    }
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
