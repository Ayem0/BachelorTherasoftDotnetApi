using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
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
        string? phoneNumber, string? description, string? address, string? city, string? country, DateTime? dateOfBirth)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var participantCategory = await _participantCategoryRepository.GetByIdAsync(participantCategoryId);
        if (participantCategory == null) return null;

        var participant = new Participant(workspace, participantCategory, firstName, lastName, description, email, phoneNumber, address, city, country, dateOfBirth)
        {
            ParticipantCategory = participantCategory,
            Workspace = workspace
        };
        await _participantRepository.CreateAsync(participant);

        return new ParticipantDto(participant);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null) return false;

        await _participantRepository.DeleteAsync(participant);
        return true;
    }

    public async Task<ParticipantDto?> GetByIdAsync(string id)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null) return null;

        return new ParticipantDto(participant);
    }

    public async Task<ParticipantDto?> UpdateAsync(string id, string? newParticipantCategoryId, string? newFirstName, string? newLastName, string? newEmail,
        string? newDescription, string? newAddress, string? newCity, string? newCountry, DateTime? newDateOfBirth)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null || (newParticipantCategoryId == null && newFirstName == null && newLastName == null && newEmail == null &&
            newDescription == null && newAddress == null && newCity == null && newCountry == null && newDateOfBirth == null)) return null;

        if (newParticipantCategoryId != null)
        {
            var participantCategory = await _participantCategoryRepository.GetByIdAsync(newParticipantCategoryId);
            if (participantCategory == null) return null;

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
        return new ParticipantDto(participant);
    }
}
