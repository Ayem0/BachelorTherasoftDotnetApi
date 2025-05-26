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
    private readonly ISocketService _socket;

    public ParticipantService(
        IParticipantRepository participantRepository,
        IWorkspaceRepository workspaceRepository,
        IParticipantCategoryRepository participantCategoryRepository,
        IMapper mapper,
        ISocketService hub
    )
    {
        _participantRepository = participantRepository;
        _workspaceRepository = workspaceRepository;
        _participantCategoryRepository = participantCategoryRepository;
        _mapper = mapper;
        _socket = hub;
    }

    public async Task<ParticipantJoinCategoryDto> CreateAsync(CreateParticipantRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(req.ParticipantCategoryId)
            ?? throw new NotFoundException("ParticipantCategory", req.ParticipantCategoryId);
        var participant = new Participant(workspace, participantCategory, req.FirstName, req.LastName, req.Description, req.Email, req.PhoneNumber,
            req.Address, req.City, req.Country, req.DateOfBirth)
        {
            ParticipantCategory = participantCategory,
            Workspace = workspace
        };
        var created = await _participantRepository.CreateAsync(participant);
        var dto = _mapper.Map<ParticipantJoinCategoryDto>(created);
        await _socket.NotififyGroup(req.WorkspaceId, "ParticipantCreated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var participant = await _participantRepository.GetByIdAsync(id) ?? throw new NotFoundException("Participant", id);
        var success = await _participantRepository.DeleteAsync(participant);
        if (success)
        {
            await _socket.NotififyGroup(participant.WorkspaceId, "ParticipantDeleted", id);
        }
        return success;
    }

    public async Task<ParticipantDto?> GetByIdAsync(string id)
    => _mapper.Map<ParticipantJoinCategoryDto>(await _participantRepository.GetByIdAsync(id));

    public async Task<List<ParticipantDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<ParticipantDto>>(await _participantRepository.GetByWorkpaceIdAsync(workspaceId));

    public async Task<List<ParticipantJoinCategoryDto>> GetByWorkspaceIdJoinCategoryAsync(string workspaceId)
    => _mapper.Map<List<ParticipantJoinCategoryDto>>(await _participantRepository.GetByWorkpaceIdJoinCategoryAsync(workspaceId));

    public async Task<ParticipantJoinCategoryDto> UpdateAsync(string id, UpdateParticipantRequest req)
    {
        var participant = await _participantRepository.GetByIdAsync(id) ?? throw new NotFoundException("Participant", id);

        if (req.ParticipantCategoryId != null)
        {
            var participantCategory = await _participantCategoryRepository.GetByIdAsync(req.ParticipantCategoryId)
                ?? throw new NotFoundException("ParticipantCategory", req.ParticipantCategoryId);
            participant.ParticipantCategory = participantCategory;
            participant.ParticipantCategoryId = participantCategory.Id;
        }

        participant.FirstName = req.FirstName ?? participant.FirstName;
        participant.LastName = req.LastName ?? participant.LastName;
        participant.Email = req.Email ?? participant.Email;
        participant.Description = req.Description ?? participant.Description;
        participant.Address = req.Address ?? participant.Address;
        participant.City = req.City ?? participant.City;
        participant.Country = req.Country ?? participant.Country;
        participant.DateOfBirth = req.DateOfBirth ?? participant.DateOfBirth;

        var updated = await _participantRepository.UpdateAsync(participant);
        var dto = _mapper.Map<ParticipantJoinCategoryDto>(updated);
        await _socket.NotififyGroup(updated.WorkspaceId, "ParticipantUpdated", dto);
        return dto;
    }
}
