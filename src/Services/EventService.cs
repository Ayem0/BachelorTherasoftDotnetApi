using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;

    public EventService(
        IEventRepository eventRepository,
        IEventCategoryRepository eventCategoryRepository,
        IRoomRepository roomRepository,
        IMapper mapper,
        IParticipantRepository participantRepository,
        ITagRepository tagRepository,
        IWorkspaceRepository workspaceRepository,
        IUserRepository userRepository,
        ISocketService socket
    )
    {
        _eventRepository = eventRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _participantRepository = participantRepository;
        _tagRepository = tagRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _socket = socket;
    }

    public async Task<EventDto> CreateAsync(string userId, CreateEventRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(req.EventCategoryId) ?? throw new NotFoundException("eventCategory", req.EventCategoryId);
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("User", userId);
        var room = await _roomRepository.GetJoinEventsSlotsByRangeAndIdAsync(req.RoomId, req.StartDate, req.EndDate) ?? throw new NotFoundException("Room", req.RoomId);
        var participants = await _participantRepository.GetByIdsAsync(req.ParticipantIds);
        var tags = await _tagRepository.GetByIdsAsync(req.TagIds);
        var users = await _userRepository.GetByIdsAsync(req.UserIds);

        var @event = new Event(workspace, req.Description, req.StartDate, req.EndDate, room, eventCategory, participants, [], tags, null, null, null, null)
        {
            Workspace = workspace,
            Room = room,
            EventCategory = eventCategory
        };

        // TODO A REFAIRE, faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event...
        // var canAdd = CanAddEvent(room, eventToAdd);
        // if (!canAdd) throw new BadRequestException("Cannot add event.", "Cannot add event.");

        @event.Users = [.. users.Select(u => new EventUser(u, @event)
        {
            Event = @event,
            User = u
        })];

        var created = await _eventRepository.CreateAsync(@event);
        var dto = _mapper.Map<EventDto>(created);

        return dto;
    }

    public async Task<List<EventDto>> CreateWithRepetitionAsync(string userId, CreateEventWithRepetitionRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(req.EventCategoryId) ?? throw new NotFoundException("eventCategory", req.EventCategoryId);
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("User", userId);
        var room = await _roomRepository.GetJoinEventsSlotsByRangeAndIdAsync(req.RoomId, req.StartDate, req.EndDate) ?? throw new NotFoundException("Room", req.RoomId);
        var participants = await _participantRepository.GetByIdsAsync(req.ParticipantIds);
        var tags = await _tagRepository.GetByIdsAsync(req.TagIds);
        var users = await _userRepository.GetByIdsAsync(req.UserIds);

        var mainEvent = new Event(room.Workspace, req.Description, req.StartDate, req.EndDate, room, eventCategory, participants, [], tags,
            req.RepetitionInterval, req.RepetitionNumber, null, req.RepetitionEndDate)
        {
            Workspace = room.Workspace,
            Room = room,
            EventCategory = eventCategory
        };

        mainEvent.Users = [.. users.Select(u => new EventUser(u, mainEvent)
        {
            Event = mainEvent,
            User = u
        })];

        // TODO A REFAIRE, faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event...
        // var canAdd = CanAddEvent(room, eventToAdd);
        // if (!canAdd) throw new BadRequestException("Cannot add event.", "Cannot add event.");

        List<Event> events = [mainEvent];

        var repetitionStartDate = Repetition.IncrementDateTime(req.StartDate, req.RepetitionInterval, req.RepetitionNumber);
        var repetitionEndDate = Repetition.IncrementDateTime(req.EndDate, req.RepetitionInterval, req.RepetitionNumber);

        while (repetitionStartDate < req.RepetitionEndDate)
        {
            var @event = new Event(room.Workspace, req.Description, repetitionStartDate, repetitionEndDate, room, eventCategory, participants, [], tags,
                null, null, mainEvent, null)
            {
                Workspace = room.Workspace,
                Room = room,
                EventCategory = eventCategory
            };
            @event.Users = [.. users.Select(u => new EventUser(u, @event)
            {
                Event = @event,
                User = u
            })];
            // TODO A REFAIRE, faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event...
            // var canAdd = CanAddEvent(room, eventToAdd);
            // if (!canAdd) throw new BadRequestException("Cannot add event.", "Cannot add event.");
            events.Add(@event);

            repetitionStartDate = Repetition.IncrementDateTime(repetitionStartDate, req.RepetitionInterval, req.RepetitionNumber);
            repetitionEndDate = Repetition.IncrementDateTime(repetitionEndDate, req.RepetitionInterval, req.RepetitionNumber);
        }
        var created = await _eventRepository.CreateMultipleAsync(events);
        var dto = _mapper.Map<List<EventDto>>(created);

        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var @event = await _eventRepository.GetByIdAsync(id) ?? throw new NotFoundException("Event", id);
        var success = await _eventRepository.DeleteAsync(@event);
        if (success)
        {
            await _socket.NotififyGroup(@event.Id, "EventDeleted", id);
        }
        return success;
    }


    public async Task<EventDto> UpdateAsync(string id, UpdateEventRequest req)
    {
        var @event = await _eventRepository.GetByIdJoinRelationsAsync(id) ?? throw new NotFoundException("Event", id);

        if (req.RoomId != null)
        {
            var room = await _roomRepository.GetByIdAsync(req.RoomId) ?? throw new NotFoundException("Room", req.RoomId);

            @event.Room = room;
            @event.RoomId = room.Id;
        }

        if (req.EventCategoryId != null)
        {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(req.EventCategoryId) ?? throw new NotFoundException("EventCategory", req.EventCategoryId);

            @event.EventCategory = eventCategory;
            @event.EventCategoryId = eventCategory.Id;
        }

        if (req.ParticipantIds != null)
        {
            var participants = await _participantRepository.GetByIdsAsync(req.ParticipantIds);
            @event.Participants = participants;
        }

        if (req.UserIds != null)
        {
            var users = await _userRepository.GetByIdsAsync(req.UserIds);
            @event.Users = [.. users.Select(u => new EventUser(u, @event)
            {
                Event = @event,
                User = u
            })];
        }

        if (req.TagIds != null)
        {
            var tags = await _tagRepository.GetByIdsAsync(req.TagIds);
            @event.Tags = tags;
        }
        ;

        @event.StartDate = req.StartDate ?? @event.StartDate;
        @event.EndDate = req.EndDate ?? @event.EndDate;
        @event.Description = req.Description ?? @event.Description;

        await _eventRepository.UpdateAsync(@event);

        return _mapper.Map<EventDto>(@event);
    }

    public async Task<EventDto?> GetByIdAsync(string id)
     => _mapper.Map<EventDto?>(await _eventRepository.GetByIdJoinRelationsAsync(id));

    public async Task<List<EventDto>> GetByRangeAndUserIdAsync(string id, DateTime start, DateTime end)
    => _mapper.Map<List<EventDto>>(await _eventRepository.GetByRangeAndUserIdJoinRelationsAsync(id, start, end));

    public async Task<List<EventDto>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end)
    => _mapper.Map<List<EventDto>>(await _eventRepository.GetByRangeAndRoomIdAsync(id, start, end));

    public async Task<DialogEventDto> GetEventsByUserIdsAndRoomIdAsync(List<string> userIds, string roomId, DateTime start, DateTime end)
    {
        Dictionary<string, List<EventDto>> usersEvents = [];
        foreach (var userId in userIds)
        {
            var userEvents = await GetByRangeAndUserIdAsync(userId, start, end);
            usersEvents[userId] = userEvents;
        }
        var roomEvents = await GetByRangeAndRoomIdAsync(roomId, start, end);
        return new DialogEventDto() { RoomEvents = roomEvents, UsersEvents = usersEvents };
    }
}
