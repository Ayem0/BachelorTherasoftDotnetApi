using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.Utils;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver.Linq;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    public EventService(IEventRepository eventRepository, IEventCategoryRepository eventCategoryRepository, IRoomRepository roomRepository, IMapper mapper,
        IParticipantRepository participantRepository, ITagRepository tagRepository, IWorkspaceRepository workspaceRepository, UserManager<User> userManager)
    {
        _eventRepository = eventRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _participantRepository = participantRepository;
        _tagRepository = tagRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<EventDto?> CreateAsync(string userId, CreateEventRequest req)
    {
        var room = await _roomRepository.GetJoinEventsSlotsByIdAsync(req.RoomId) ?? throw new NotFoundException("Room", req.RoomId);
        var workspace = await _workspaceRepository.GetByIdAsync(room.WorkspaceId) ?? throw new NotFoundException("Workspace", room.WorkspaceId);
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(req.EventCategoryId) ?? throw new NotFoundException("EventCategory", req.EventCategoryId);
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException("User", userId);
        List<Participant> participants = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < req.ParticipantIds?.Count; i++)
        {
            var participant = await _participantRepository.GetByIdAsync(req.ParticipantIds[i]) ?? throw new NotFoundException("Participant", req.RoomId);

            if (!participants.Contains(participant)) participants.Add(participant);
        }

        List<Tag> tags = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < req.TagIds?.Count; i++)
        {
            var tag = await _tagRepository.GetByIdAsync(req.TagIds[i]) ?? throw new NotFoundException("Tag", req.TagIds[i]);

            if (!tags.Contains(tag)) tags.Add(tag);
        }

        var eventToAdd = new Event(workspace, req.Description, req.StartDate, req.EndDate, room, eventCategory, participants, [], tags, null, null, null, null)
        {
            Workspace = workspace,
            Room = room,
            EventCategory = eventCategory
        };
        var canAdd = CanAddEvent(room, eventToAdd);
        // TODO faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event
        if (!canAdd) throw new BadRequestException("Cannot add event.", "Cannot add event.");

        var eventUser = new EventUser(user, eventToAdd)
        {
            Event = eventToAdd,
            User = user
        };
        eventToAdd.Users.Add(eventUser);

        await _eventRepository.CreateAsync(eventToAdd);

        return _mapper.Map<EventDto>(eventToAdd);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _eventRepository.DeleteAsync(id);
    }

    public async Task<EventDto> GetByIdAsync(string id)
    {
        var eventToGet = await _eventRepository.GetByIdAsync(id) ?? throw new NotFoundException("Event", id);

        return _mapper.Map<EventDto>(eventToGet);
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



    // TODO renvoyer un message d'erreur avec les slot et/ou les events qui pose problemes
    private static bool CanAddEvent(Room room, Event @event)
    {
        var eventStartDate = new DateOnly(@event.StartDate.Year, @event.StartDate.Month, @event.StartDate.Day);
        var eventEndDate = new DateOnly(@event.EndDate.Year, @event.EndDate.Month, @event.EndDate.Day);

        var eventStartTime = new TimeOnly(@event.StartDate.Hour, @event.StartDate.Minute, @event.StartDate.Second);
        var eventEndTime = new TimeOnly(@event.EndDate.Hour, @event.EndDate.Minute, @event.EndDate.Second);

        var roomSlots = room.Slots.Where(existingSlot => existingSlot.DeletedAt == null &&
            (existingSlot.StartDate <= eventStartDate && existingSlot.EndDate >= eventEndDate && existingSlot.StartTime <= eventStartTime && existingSlot.EndTime >= eventEndTime ||

            existingSlot.StartDate > eventStartDate && existingSlot.EndDate < eventEndDate && existingSlot.StartTime > eventStartTime && existingSlot.EndTime < eventEndTime ||

            existingSlot.StartDate < eventStartDate && existingSlot.EndDate > eventStartDate && existingSlot.EndDate < eventEndDate && existingSlot.StartTime < eventStartTime && existingSlot.EndTime > eventStartTime && existingSlot.EndTime < eventEndTime ||

            existingSlot.EndDate < eventEndDate && existingSlot.StartDate > eventStartDate && existingSlot.StartDate < eventEndDate && existingSlot.EndTime < eventEndTime && existingSlot.StartTime > eventStartTime && existingSlot.StartTime < eventEndTime)).ToList();

        if (roomSlots.Count > 0)
        {
            foreach (var roomSlot in roomSlots)
            {
                if (roomSlot.EventCategories.Count > 0 && !roomSlot.EventCategories.Contains(@event.EventCategory)) return false;
            }
        }

        var roomEvents = room.Events.Where(existingEvent => existingEvent.DeletedAt == null &&
            // event with same date or starting before and ending after
            (existingEvent.StartDate <= @event.StartDate && existingEvent.EndDate >= @event.EndDate ||
            // event starting after and ending before
            existingEvent.StartDate > @event.StartDate && existingEvent.EndDate < @event.EndDate ||
            // event starting before and ending before
            existingEvent.StartDate < @event.StartDate && existingEvent.EndDate > @event.StartDate && existingEvent.EndDate < @event.EndDate ||
            // event starting after and ending after
            existingEvent.StartDate > @event.StartDate && existingEvent.EndDate > @event.EndDate && existingEvent.StartDate < @event.EndDate)).ToList();

        return roomEvents.Count == 0;

    }

    public async Task<List<EventDto>?> CreateWithRepetitionAsync(CreateEventWithRepetitionRequest request)
    {
        var room = await _roomRepository.GetJoinEventsSlotsByIdAsync(request.RoomId);
        if (room == null) return null;

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(request.EventCategoryId);
        if (eventCategory == null) return null;

        List<Participant> participants = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < request.ParticipantIds?.Count; i++)
        {
            var participant = await _participantRepository.GetByIdAsync(request.ParticipantIds[i]);
            if (participant == null) return null;

            if (!participants.Contains(participant)) participants.Add(participant);
        }

        List<Tag> tags = [];
        for (int i = 0; i < request.TagIds?.Count; i++)
        {
            var tag = await _tagRepository.GetByIdAsync(request.TagIds[i]);
            if (tag == null) return null;

            if (!tags.Contains(tag)) tags.Add(tag);
        }

        var mainEvent = new Event(room.Workspace, request.Description, request.StartDate, request.EndDate, room, eventCategory, participants, [], tags,
            request.RepetitionInterval, request.RepetitionNumber, null, request.RepetitionEndDate)
        {
            Workspace = room.Workspace,
            Room = room,
            EventCategory = eventCategory
        };

        var canAdd = CanAddEvent(room, mainEvent);
        // TODO changer le type de response de canAdd et récupérer l'erreur en dessous a la place de todo
        if (!canAdd) return null;

        List<Event> events = [mainEvent];

        var repetitionStartDate = Repetition.IncrementDateTime(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = Repetition.IncrementDateTime(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

        while (DateOnly.FromDateTime(repetitionStartDate) < request.RepetitionEndDate)
        {
            var @event = new Event(room.Workspace, request.Description, repetitionStartDate, repetitionEndDate, room, eventCategory, participants, [], tags,
                null, null, mainEvent, null)
            {
                Workspace = room.Workspace,
                Room = room,
                EventCategory = eventCategory
            };

            bool canAddEvent = CanAddEvent(room, @event);
            // TODO changer le type de response de canAdd et récupérer l'erreur en dessous a la place de todo
            if (!canAddEvent) return null;

            events.Add(@event);

            repetitionStartDate = Repetition.IncrementDateTime(repetitionStartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = Repetition.IncrementDateTime(repetitionEndDate, request.RepetitionInterval, request.RepetitionNumber);
        }

        await _eventRepository.CreateMultipleAsync(events);

        return _mapper.Map<List<EventDto>>(events);
    }


    public async Task<List<EventDto>> GetByRangeAndUserIdAsync(string id, DateTime start, DateTime end)
    {
        var res = await _eventRepository.GetByRangeAndUserIdAsync(id, start, end);
        return _mapper.Map<List<EventDto>>(res);
    }

    public async Task<List<EventDto>> GetByRangeAndRoomIdAsync(string id, DateTime start, DateTime end)
    {
        var res = await _eventRepository.GetByRangeAndRoomIdAsync(id, start, end);
        return _mapper.Map<List<EventDto>>(res);
    }

    public async Task<List<EventDto>> GetEventsByUserIdsAndRoomIdAsync(List<string> userIds, string roomId, DateTime start, DateTime end)
    {
        var res = await _eventRepository.GetEventsByUserIdsAndRoomIdAsync(userIds, roomId, start, end);
        return _mapper.Map<List<EventDto>>(res);
    }
}
