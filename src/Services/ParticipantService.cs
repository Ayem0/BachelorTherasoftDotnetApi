using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public ParticipantService(IParticipantRepository participantRepository, IWorkspaceRepository workspaceRepository, IParticipantCategoryRepository participantCategoryRepository, IMapper mapper)
    {
        _participantRepository = participantRepository;
        _workspaceRepository = workspaceRepository;
        _participantCategoryRepository = participantCategoryRepository;
        _mapper = mapper;
    }

    public async Task<ParticipantDto> CreateAsync(CreateParticipantRequest request)
    {
        var workspace = await _workspaceRepository.GetEntityByIdAsync(request.WorkspaceId) ?? throw new NotFoundException("Workspace", request.WorkspaceId);

        var participantCategory = await _participantCategoryRepository.GetEntityByIdAsync(request.ParticipantCategoryId) ?? throw new NotFoundException("ParticipantCategory", request.ParticipantCategoryId);

        var participant = new Participant(workspace, participantCategory, request.FirstName, request.LastName, request.Description, request.Email, request.PhoneNumber, 
            request.Address, request.City, request.Country, request.DateOfBirth)
        {
            ParticipantCategory = participantCategory,
            Workspace = workspace
        };

        await _participantRepository.CreateAsync(participant);

        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _participantRepository.DeleteAsync(id);
    }

    public async Task<ParticipantDto> GetByIdAsync(string id)
    {
        var participant = await _participantRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("Participant", id);

        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task<ParticipantDto> UpdateAsync(string id, UpdateParticipantRequest request)
    {
        var participant = await _participantRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("Participant", id);

        if (request.NewParticipantCategoryId != null)
        {
            var participantCategory = await _participantCategoryRepository.GetEntityByIdAsync(request.NewParticipantCategoryId) 
                ?? throw new NotFoundException("ParticipantCategory", request.NewParticipantCategoryId);

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

        return _mapper.Map<ParticipantDto>(participant);
    }
}
