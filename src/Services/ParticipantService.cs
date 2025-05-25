using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public ParticipantService(
        IParticipantRepository participantRepository,
        IWorkspaceRepository workspaceRepository,
        IParticipantCategoryRepository participantCategoryRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService hub
    )
    {
        _participantRepository = participantRepository;
        _workspaceRepository = workspaceRepository;
        _participantCategoryRepository = participantCategoryRepository;
        _mapper = mapper;
        _socket = hub;
        _cache = cache;
    }

    public async Task<ParticipantJoinCategoryDto> CreateAsync(string workspaceId, CreateParticipantRequest request)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var participantCategory = await _cache.GetOrSetAsync(
            CacheKeys.ParticipantCategory(workspaceId, request.ParticipantCategoryId),
            () => _participantCategoryRepository.GetByIdAsync(request.ParticipantCategoryId),
            ttl
        ) ?? throw new NotFoundException("ParticipantCategory", request.ParticipantCategoryId);

        var participant = new Participant(workspace, participantCategory, request.FirstName, request.LastName, request.Description, request.Email, request.PhoneNumber,
            request.Address, request.City, request.Country, request.DateOfBirth)
        {
            ParticipantCategory = participantCategory,
            Workspace = workspace
        };

        var created = await _participantRepository.CreateAsync(participant);
        var dto = _mapper.Map<ParticipantJoinCategoryDto>(created);

        await _socket.NotififyGroup(workspaceId, "ParticipantCreated", dto);
        await _cache.SetAsync(CacheKeys.Participant(workspaceId, created.Id), created, ttl);

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var participant = await _cache.GetOrSetAsync(
            CacheKeys.Participant(workspaceId, id),
            () => _participantRepository.GetByIdAsync(id),
            ttl
        ) ?? throw new NotFoundException("Participant", id);

        var success = await _participantRepository.DeleteAsync(participant);
        if (success)
        {
            await _socket.NotififyGroup(participant.WorkspaceId, "ParticipantDeleted", id);
            await _cache.DeleteAsync(CacheKeys.Participant(workspaceId, id));
        }
        return success;
    }

    public Task<ParticipantDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<Participant?, ParticipantDto?>(
        CacheKeys.Participant(workspaceId, id),
        () => _participantRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<ParticipantDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Participant>, List<ParticipantDto>>(
        CacheKeys.Participants(workspaceId),
        () => _participantRepository.GetByWorkpaceIdAsync(workspaceId),
        ttl
    );

    public Task<List<ParticipantJoinCategoryDto>> GetByWorkspaceIdJoinCategoryAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Participant>, List<ParticipantJoinCategoryDto>>(
        CacheKeys.ParticipantsJoinCategory(workspaceId),
        () => _participantRepository.GetByWorkpaceIdJoinCategoryAsync(workspaceId),
        ttl
    );

    public async Task<ParticipantJoinCategoryDto> UpdateAsync(string workspaceId, string id, UpdateParticipantRequest req)
    {
        var participant = await _cache.GetOrSetAsync(
            CacheKeys.Participant(workspaceId, id),
            () => _participantRepository.GetByIdAsync(id),
            ttl
        ) ?? throw new NotFoundException("Participant", id);

        if (req.ParticipantCategoryId != null)
        {
            var participantCategory = await _cache.GetOrSetAsync(
                CacheKeys.ParticipantCategory(workspaceId, req.ParticipantCategoryId),
                () => _participantCategoryRepository.GetByIdAsync(req.ParticipantCategoryId),
                ttl
            ) ?? throw new NotFoundException("ParticipantCategory", req.ParticipantCategoryId);

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
        var dto = _mapper.Map<ParticipantJoinCategoryDto>(participant);

        await _socket.NotififyGroup(workspaceId, "ParticipantUpdated", dto);
        await _cache.SetAsync(CacheKeys.Participant(workspaceId, id), updated, ttl);

        return dto;
    }
}
