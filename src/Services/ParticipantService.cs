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

    public async Task<ParticipantJoinCategoryDto> CreateAsync(CreateParticipantRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId) ?? throw new NotFoundException("Workspace", request.WorkspaceId);

        var participantCategory = await _participantCategoryRepository.GetByIdAsync(request.ParticipantCategoryId) ?? throw new NotFoundException("ParticipantCategory", request.ParticipantCategoryId);

        var participant = new Participant(workspace, participantCategory, request.FirstName, request.LastName, request.Description, request.Email, request.PhoneNumber,
            request.Address, request.City, request.Country, request.DateOfBirth)
        {
            ParticipantCategory = participantCategory,
            Workspace = workspace
        };

        await _participantRepository.CreateAsync(participant);

        return _mapper.Map<ParticipantJoinCategoryDto>(participant);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _participantRepository.DeleteAsync(id);
    }

    public async Task<ParticipantDto> GetByIdAsync(string id)
    {
        var participant = await _participantRepository.GetByIdAsync(id) ?? throw new NotFoundException("Participant", id);

        return _mapper.Map<ParticipantDto>(participant);
    }

    public async Task<ParticipantJoinCategoryDto> UpdateAsync(string id, UpdateParticipantRequest request)
    {
        var participant = await _participantRepository.GetByIdAsync(id) ?? throw new NotFoundException("Participant", id);

        participant.ParticipantCategory = request.ParticipantCategoryId != null
            ? await _participantCategoryRepository.GetByIdAsync(request.ParticipantCategoryId)
            ?? throw new NotFoundException("ParticipantCategory", request.ParticipantCategoryId)
            : participant.ParticipantCategory;
        participant.ParticipantCategoryId = request.ParticipantCategoryId ?? participant.ParticipantCategoryId;
        participant.FirstName = request.FirstName ?? participant.FirstName;
        participant.LastName = request.LastName ?? participant.LastName;
        participant.Email = request.Email ?? participant.Email;
        participant.Description = request.Description ?? participant.Description;
        participant.Address = request.Address ?? participant.Address;
        participant.City = request.City ?? participant.City;
        participant.Country = request.Country ?? participant.Country;
        participant.DateOfBirth = request.DateOfBirth ?? participant.DateOfBirth;

        await _participantRepository.UpdateAsync(participant);

        return _mapper.Map<ParticipantJoinCategoryDto>(participant);
    }

    public async Task<List<ParticipantDto>> GetByWorkspaceIdAsync(string id)
    {
        var res = await _participantRepository.GetByWorkpaceIdAsync(id);
        return _mapper.Map<List<ParticipantDto>>(res);
    }

    public async Task<List<ParticipantJoinCategoryDto>> GetByWorkspaceIdJoinCategoryAsync(string id)
    {
        var res = await _participantRepository.GetByWorkpaceIdJoinCategoryAsync(id);
        return _mapper.Map<List<ParticipantJoinCategoryDto>>(res);
    }
}
