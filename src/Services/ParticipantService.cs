using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    public ParticipantService(IParticipantRepository participantRepository, IWorkspaceRepository workspaceRepository, IParticipantCategoryRepository participantCategoryRepository)
    {
        _participantRepository = participantRepository;
        _workspaceRepository = workspaceRepository;
        _participantCategoryRepository = participantCategoryRepository;
    }

    public async Task<ParticipantDto?> CreateAsync(string workspaceId, string participantCategoryId, string firstName, string lastName, string? email,
        string? description, string? address, string? city, string? country, DateTime? dateOfBirth)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if(workspace == null) return null;

        var participantCategory = await _participantCategoryRepository.GetByIdAsync(participantCategoryId);
        if(participantCategory == null) return null;

        var participant = new Participant {
            FirstName = firstName,
            LastName = lastName,
            Email = email, 
            Description = description,
            Address = address,
            City = city,
            Country = country,
            DateOfBirth = dateOfBirth,
            ParticipantCategory = participantCategory,
            ParticipantCategoryId = participantCategory.Id,
            Workspace = workspace,
            WorkspaceId = workspace.Id
        };

        await _participantRepository.CreateAsync(participant);

        var participantDto = new ParticipantDto {
            Id = participant.Id,
            FirstName = participant.FirstName,
            LastName = participant.LastName,
            Email = participant.Email,
            Description = participant.Description,
            Address = participant.Address,
            City = participant.City,
            Country = participant.Country,
            DateOfBirth = participant.DateOfBirth
        };

        return participantDto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var participant  = await _participantRepository.GetByIdAsync(id);
        if(participant == null) return false;

        await _participantRepository.DeleteAsync(participant);
        return true;
    }

    public async Task<ParticipantDto?> GetByIdAsync(string id)
    {
        var participant  = await _participantRepository.GetByIdAsync(id);
        if(participant == null) return null;

        var participantDto = new ParticipantDto {
            Id = participant.Id,
            FirstName = participant.FirstName,
            LastName = participant.LastName,
            Email = participant.Email,
            Description = participant.Description,
            Address = participant.Address,
            City = participant.City,
            Country = participant.Country,
            DateOfBirth = participant.DateOfBirth
        };

        return participantDto;
    }

    public async Task<bool> UpdateAsync(string id, string? newParticipantCategoryId, string? newFirstName, string? newLastName, string? newEmail, 
        string? newDescription, string? newAddress, string? newCity, string? newCountry, DateTime? newDateOfBirth)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        if(participant == null || (newParticipantCategoryId == null && newFirstName == null && newLastName == null && newEmail == null && 
            newDescription == null && newAddress == null && newCity == null && newCountry == null && newDateOfBirth == null)) return false;

        if (newParticipantCategoryId != null) {
            var participantCategory = await _participantCategoryRepository.GetByIdAsync(newParticipantCategoryId);
            if (participantCategory == null) return false;

            participant.ParticipantCategory = participantCategory;
            participant.ParticipantCategoryId = participantCategory.Id;
        }

        participant.FirstName = newFirstName ?? participant.FirstName;
        participant.LastName = newLastName ?? participant.LastName;
        participant.Email = newEmail ?? participant.Email;
        participant.Description = newDescription ?? participant.Description;
        participant.Address = newAddress ?? participant.Address;
        participant.City = newCity ?? participant.City;
        participant.Country = newCountry ?? participant.Country;
        participant.DateOfBirth = newDateOfBirth ?? participant.DateOfBirth;

        await _participantRepository.UpdateAsync(participant);
        
        return true;
    }
}
