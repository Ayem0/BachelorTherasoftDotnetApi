using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
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

    public async Task<Response<ParticipantDto?>> CreateAsync(CreateParticipantRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new Response<ParticipantDto?>(success: false, errors: ["Participant not found."]);

        var participantCategory = await _participantCategoryRepository.GetByIdAsync(request.ParticipantCategoryId);
        if (participantCategory == null) return new Response<ParticipantDto?>(success: false, errors: ["Participant category not found."]);

        var participant = new Participant(workspace, participantCategory, request.FirstName, request.LastName, request.Description, request.Email, request.PhoneNumber, request.Address, request.City, request.Country, request.DateOfBirth)
        {
            ParticipantCategory = participantCategory,
            Workspace = workspace
        };
        await _participantRepository.CreateAsync(participant);

        return new Response<ParticipantDto?>(success: true, content: new ParticipantDto(participant));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null) return new Response<string>(success: false, errors: ["Participant not found."]);

        await _participantRepository.DeleteAsync(participant);
        return new Response<string>(success: true, content: "Participant successfully deleted.");
    }

    public async Task<Response<ParticipantDto?>> GetByIdAsync(string id)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null) return new Response<ParticipantDto?>(success: false, errors: ["Participant not found."]);

        return new Response<ParticipantDto?>(success: true, content: new ParticipantDto(participant));
    }

    public async Task<Response<ParticipantDto?>> UpdateAsync(string id, UpdateParticipantRequest request)
    {
        if (request.NewParticipantCategoryId == null && request.NewFirstName == null && request.NewLastName == null && request.NewEmail == null &&
            request.NewDescription == null && request.NewAddress == null && request.NewCity == null && request.NewCountry == null && request.NewDateOfBirth == null) 
            return new Response<ParticipantDto?>(success: false, errors: ["At least one field is required."]);
            
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null) return new Response<ParticipantDto?>(success: false, errors: ["Participant not found."]);

        if (request.NewParticipantCategoryId != null)
        {
            var participantCategory = await _participantCategoryRepository.GetByIdAsync(request.NewParticipantCategoryId);
            if (participantCategory == null) return new Response<ParticipantDto?>(success: false, errors: ["Participant category not found."]);

            participant.ParticipantCategory = participantCategory;
            participant.ParticipantCategoryId = participantCategory.Id;
        }

        participant.FirstName = request.NewFirstName ?? participant.FirstName;
        participant.LastName = request.NewLastName ?? participant.LastName;
        participant.Email = request.NewEmail ?? participant.Email;
        participant.Description = request.NewDescription ?? participant.Description;
        participant.Address = request.NewAddress ?? participant.Address;
        participant.City = request.NewCity ?? participant.City;
        participant.Country = request.NewCountry ?? participant.Country;
        participant.DateOfBirth = request.NewDateOfBirth ?? participant.DateOfBirth;

        await _participantRepository.UpdateAsync(participant);

        return new Response<ParticipantDto?>(success: true, content: new ParticipantDto(participant));
    }
}
