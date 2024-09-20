using System;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IParticipantRepository
{
    Task CreateParticipantAsync(string firstName, string lastName, string? email, string? country, string? description, DateTime? dateOfBirth, string? participantCategoryId);
    Task GetParticipantAsync(string participantId);
    Task GetParticipantsAsync(string[] participantIds);
    Task UpdateParticipantAsync(string participantId, string firstName, string lastName, string? email, string? country, string? description, DateTime? dateOfBirth, string? participantCategoryId);
    Task DeleteParticipantAsync(string participantId);
    Task DeleteParticipantsAsync(string[] participantIds);
}
