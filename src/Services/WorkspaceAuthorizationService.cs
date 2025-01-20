using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;

namespace BachelorTherasoftDotnetApi.src.Services;

public class WorkspaceAuthorizationService : IWorkspaceAuthorizationService
{
    // TODO ajouter le membre repository une fois finis
    private readonly ILocationRepository _locationRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IWorkspaceRoleRepository _workspaceRoleRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ISlotRepository _slotRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    public WorkspaceAuthorizationService(
        ILocationRepository locationRepository, 
        ITagRepository tagRepository, 
        IWorkspaceRoleRepository workspaceRoleRepository, 
        IWorkspaceRepository workspaceRepository, 
        ISlotRepository slotRepository,
        IParticipantRepository participantRepository,
        IEventCategoryRepository eventCategoryRepository,
        IParticipantCategoryRepository participantCategoryRepository,
        IAreaRepository areaRepository,
        IRoomRepository roomRepository,
        IEventRepository eventRepository
    ){
        _locationRepository = locationRepository;
        _workspaceRepository = workspaceRepository;
        _workspaceRoleRepository = workspaceRoleRepository;
        _slotRepository = slotRepository;
        _participantRepository = participantRepository;
        _participantCategoryRepository = participantCategoryRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _tagRepository = tagRepository;
        _roomRepository = roomRepository;
        _eventRepository = eventRepository;
        _areaRepository = areaRepository;
}

    public async Task<BaseAuthorizationModel?> GetEntityById(string tableName, string id) 
    {
        return tableName switch
        {          
            "Workspace" => await _workspaceRepository.GetJoinUsersByIdAsync(id),
            "WorkspaceRole" => await _workspaceRoleRepository.GetEntityByIdAsync(id),
            "Tag" => await _tagRepository.GetEntityByIdAsync(id),
            "Participant" => await _participantRepository.GetEntityByIdAsync(id),
            "ParticipantCategory" => await _participantCategoryRepository.GetEntityByIdAsync(id),
            "EventCategory" => await _eventCategoryRepository.GetEntityByIdAsync(id),
            "Slot" => await _slotRepository.GetEntityByIdAsync(id),
            "Event" => await _eventRepository.GetEntityByIdAsync(id),
            "Room" => await _roomRepository.GetByIdAsync(id),
            "Area" => await _areaRepository.GetEntityByIdAsync(id),
            "Location" => await _locationRepository.GetByIdAsync(id),
            _ => null,
        };
    }
}
