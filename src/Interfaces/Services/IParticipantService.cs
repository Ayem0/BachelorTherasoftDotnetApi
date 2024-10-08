using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantService
{
    Task<ParticipantDto?> GetByIdAsync(string id);
    Task<ParticipantDto?> CreateAsync(string workspaceId, string participantCategoryId, string firstName, string lastName, string? description,
        string? email, string? phoneNumber, string? address, string? city, string? country, DateTime? dateOfBirth);
    Task<bool> DeleteAsync(string id);
    Task<ParticipantDto?> UpdateAsync(string id, string? newParticipantCategoryId, string? newFirstName, string? newLastName, string? newEmail, string? newDescription,
        string? newAddress, string? newCity, string? newCountry, DateTime? newDateOfBirth);
}
