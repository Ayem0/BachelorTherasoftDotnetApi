using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
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
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public EventService(
        IEventRepository eventRepository,
        IEventCategoryRepository eventCategoryRepository,
        IRoomRepository roomRepository,
        IMapper mapper,
        IParticipantRepository participantRepository,
        ITagRepository tagRepository,
        IWorkspaceRepository workspaceRepository,
        IUserRepository userRepository,
        IRedisService cache,
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
        _cache = cache;
        _socket = socket;
    }

    public async Task<EventDto> CreateAsync(string userId, CreateEventRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(req.WorkspaceId),
            () => _workspaceRepository.GetByIdAsync(req.WorkspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", req.WorkspaceId);

        var eventCategory = await _cache.GetOrSetAsync(
            CacheKeys.EventCategory(req.WorkspaceId, req.EventCategoryId),
            () => _eventCategoryRepository.GetByIdAsync(req.EventCategoryId),
            ttl
        ) ?? throw new NotFoundException("eventCategory", req.EventCategoryId);

        var user = await _cache.GetOrSetAsync(CacheKeys.User(userId), () => _userRepository.GetByIdAsync(userId), ttl)
            ?? throw new NotFoundException("User", userId);

        var room = await _cache.GetOrSetAsync(
            CacheKeys.RoomDetails(req.WorkspaceId, req.RoomId),
            () => _roomRepository.GetJoinEventsSlotsByRangeAndIdAsync(req.RoomId, req.StartDate, req.EndDate),
            ttl
        ) ?? throw new NotFoundException("Room", req.RoomId);

        List<Participant> participants = [];
        foreach (var pId in req.ParticipantIds)
        {
            var participant = await _cache.GetOrSetAsync(
                CacheKeys.Participant(req.WorkspaceId, pId),
                () => _participantRepository.GetByIdAsync(pId),
                ttl
            ) ?? throw new NotFoundException("Participant", pId);
            participants.Add(participant);
        }

        List<Tag> tags = [];
        foreach (var tagId in req.TagIds)
        {
            var tag = await _cache.GetOrSetAsync(
                CacheKeys.Tag(req.WorkspaceId, tagId),
                () => _tagRepository.GetByIdAsync(tagId),
                ttl
            ) ?? throw new NotFoundException("Tag", tagId);
            tags.Add(tag);
        }

        List<User> users = [user];
        foreach (var uId in req.UserIds)
        {
            var User = await _cache.GetOrSetAsync(
                CacheKeys.User(uId),
                () => _userRepository.GetByIdAsync(uId),
                ttl
            ) ?? throw new NotFoundException("User", uId);
            users.Add(User);
        }

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
            User = user
        })];

        var created = await _eventRepository.CreateAsync(@event);
        var dto = _mapper.Map<EventDto>(@event);

        return dto;
    }

    public async Task<List<EventDto>> CreateWithRepetitionAsync(string userId, CreateEventWithRepetitionRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(req.WorkspaceId),
            () => _workspaceRepository.GetByIdAsync(req.WorkspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", req.WorkspaceId);

        var eventCategory = await _cache.GetOrSetAsync(
            CacheKeys.EventCategory(req.WorkspaceId, req.EventCategoryId),
            () => _eventCategoryRepository.GetByIdAsync(req.EventCategoryId),
            ttl
        ) ?? throw new NotFoundException("eventCategory", req.EventCategoryId);

        var user = await _cache.GetOrSetAsync(CacheKeys.User(userId), () => _userRepository.GetByIdAsync(userId), ttl)
            ?? throw new NotFoundException("User", userId);

        var room = await _cache.GetOrSetAsync(
            CacheKeys.RoomDetails(req.WorkspaceId, req.RoomId),
            () => _roomRepository.GetJoinEventsSlotsByRangeAndIdAsync(req.RoomId, req.StartDate, req.EndDate),
            ttl
        ) ?? throw new NotFoundException("Room", req.RoomId);

        List<Participant> participants = [];
        foreach (var pId in req.ParticipantIds)
        {
            var participant = await _cache.GetOrSetAsync(
                CacheKeys.Participant(req.WorkspaceId, pId),
                () => _participantRepository.GetByIdAsync(pId),
                ttl
            ) ?? throw new NotFoundException("Participant", pId);
            participants.Add(participant);
        }

        List<Tag> tags = [];
        foreach (var tagId in req.TagIds)
        {
            var tag = await _cache.GetOrSetAsync(
                CacheKeys.Tag(req.WorkspaceId, tagId),
                () => _tagRepository.GetByIdAsync(tagId),
                ttl
            ) ?? throw new NotFoundException("Tag", tagId);
            tags.Add(tag);
        }

        List<User> users = [user];
        foreach (var uId in req.UserIds)
        {
            var User = await _cache.GetOrSetAsync(
                CacheKeys.User(uId),
                () => _userRepository.GetByIdAsync(uId),
                ttl
            ) ?? throw new NotFoundException("User", uId);
            users.Add(User);
        }


        var mainEvent = new Event(room.Workspace, req.Description, req.StartDate, req.EndDate, room, eventCategory, participants, [], tags,
            req.RepetitionInterval, req.RepetitionNumber, null, req.RepetitionEndDate)
        {
            Workspace = room.Workspace,
            Room = room,
            EventCategory = eventCategory
        };

        var eventUsers = users.Select(u => new EventUser(u, mainEvent)
        {
            Event = mainEvent,
            User = user
        }).ToList();
        mainEvent.Users = eventUsers;

        // TODO A REFAIRE, faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event...
        // var canAdd = CanAddEvent(room, eventToAdd);
        // if (!canAdd) throw new BadRequestException("Cannot add event.", "Cannot add event.");

        List<Event> events = [mainEvent];

        var repetitionStartDate = Repetition.IncrementDateTime(req.StartDate, req.RepetitionInterval, req.RepetitionNumber);
        var repetitionEndDate = Repetition.IncrementDateTime(req.EndDate, req.RepetitionInterval, req.RepetitionNumber);

        while (DateOnly.FromDateTime(repetitionStartDate) < req.RepetitionEndDate)
        {
            var @event = new Event(room.Workspace, req.Description, repetitionStartDate, repetitionEndDate, room, eventCategory, participants, eventUsers, tags,
                null, null, mainEvent, null)
            {
                Workspace = room.Workspace,
                Room = room,
                EventCategory = eventCategory
            };
            // TODO A REFAIRE, faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event...
            // var canAdd = CanAddEvent(room, eventToAdd);
            // if (!canAdd) throw new BadRequestException("Cannot add event.", "Cannot add event.");
            events.Add(@event);

            repetitionStartDate = Repetition.IncrementDateTime(repetitionStartDate, req.RepetitionInterval, req.RepetitionNumber);
            repetitionEndDate = Repetition.IncrementDateTime(repetitionEndDate, req.RepetitionInterval, req.RepetitionNumber);
        }
        var created = await _eventRepository.CreateMultipleAsync(events);
        var dto = _mapper.Map<List<EventDto>>(events);

        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var key = CacheKeys.Event(id);
        var @event = await _cache.GetOrSetAsync(key, () => _eventRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Event", id);
        var success = await _eventRepository.DeleteAsync(@event);
        if (success)
        {
            await _socket.NotififyGroup(@event.Id, "EventDeleted", id);
            // TODO remove other keys (related users, room ...)
            await _cache.DeleteAsync(key);
        }
        return success;
    }


    public async Task<EventDto> UpdateAsync(string id, UpdateEventRequest req)
    {
        var eventToUpdate = await _eventRepository.GetByIdJoinRelationsAsync(id) ?? throw new NotFoundException("Event", id);

        if (req.NewRoomId != null)
        {
            var room = await _roomRepository.GetByIdAsync(id) ?? throw new NotFoundException("Room", req.NewRoomId);

            eventToUpdate.Room = room;
            eventToUpdate.RoomId = room.Id;
        }

        if (req.NewEventCategoryId != null)
        {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("EventCategory", req.NewEventCategoryId);

            eventToUpdate.EventCategory = eventCategory;
            eventToUpdate.EventCategoryId = eventCategory.Id;
        }

        List<Participant> participants = [];
        // TODO a refactor comme le create
        for (int i = 0; i < req.NewParticipantIds?.Count; i++)
        {
            var participantToAdd = eventToUpdate.Participants.Find(x => x.Id == req.NewParticipantIds[i]);
            if (participantToAdd == null)
            {
                var participant = await _participantRepository.GetByIdAsync(req.NewParticipantIds[i]) ?? throw new NotFoundException("Participant", req.NewParticipantIds[i]);
                participants.Add(participant);
            }
            else
            {
                participants.Add(participantToAdd);
            }
        }

        List<Tag> tags = [];
        // TODO a refactor comme le create
        for (int i = 0; i < req.NewTagIds?.Count; i++)
        {
            var tagToAdd = eventToUpdate.Tags.Find(x => x.Id == req.NewTagIds[i]);
            if (tagToAdd == null)
            {
                var tag = await _tagRepository.GetByIdAsync(req.NewTagIds[i]) ?? throw new NotFoundException("Tag", req.NewTagIds[i]);
                tags.Add(tag);
            }
            else
            {
                tags.Add(tagToAdd);
            }
        }

        eventToUpdate.StartDate = req.NewStartDate ?? eventToUpdate.StartDate;
        eventToUpdate.EndDate = req.NewEndDate ?? eventToUpdate.EndDate;
        eventToUpdate.Description = req.NewDescription ?? eventToUpdate.Description;
        eventToUpdate.Participants = participants.Count > 0 ? participants : eventToUpdate.Participants;
        eventToUpdate.Tags = tags.Count > 0 ? tags : eventToUpdate.Tags;

        await _eventRepository.UpdateAsync(eventToUpdate);

        return _mapper.Map<EventDto>(eventToUpdate);
    }

    public Task<EventDto?> GetByIdAsync(string id)
     => _cache.GetOrSetAsync<Event?, EventDto?>(
        CacheKeys.Event(id),
        () => _eventRepository.GetByIdAsync(id),
        ttl
    );


    public Task<List<EventDto>> GetByRangeAndUserIdAsync(string id, DateTime start, DateTime end)
    => _cache.GetOrSetAsync<List<Event>, List<EventDto>>(
        CacheKeys.UserEvents(id, start.ToString(), end.ToString()),
        () => _eventRepository.GetByRangeAndUserIdAsync(id, start, end),
        ttl
    );

    public Task<List<EventDto>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end)
    => _cache.GetOrSetAsync<List<Event>, List<EventDto>>(
        CacheKeys.RoomEvents(id, start.ToString(), end.ToString()),
        () => _eventRepository.GetByRangeAndRoomIdAsync(id, start, end),
        ttl
    );

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
