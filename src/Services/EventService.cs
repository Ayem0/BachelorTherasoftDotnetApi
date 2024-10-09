using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using MongoDB.Driver.Linq;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IRepetitionService _repetitionService;
    public EventService(IEventRepository eventRepository, IEventCategoryRepository eventCategoryRepository, IRoomRepository roomRepository,
        IParticipantRepository participantRepository, ITagRepository tagRepository, IRepetitionService repetitionService)
    {
        _eventRepository = eventRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _participantRepository = participantRepository;
        _tagRepository = tagRepository;
        _repetitionService = repetitionService;
    }

    public async Task<Response<EventDto?>> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate,
        List<string>? participantIds, List<string>? tagIds)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return new Response<EventDto?>(success: false, errors: ["Room not found."]);

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
        if (eventCategory == null) return new Response<EventDto?>(success: false, errors: ["Event category not found."]);

        List<Participant> participants = [];
        for (int i = 0; i < participantIds?.Count; i++)
        {
            var participant = await _participantRepository.GetByIdAsync(participantIds[i]);
            // TODO retourner l'id du participant qui pose probleme
            if (participant == null) return new Response<EventDto?>(success: false, errors: ["Participant not found."]);

            if (!participants.Contains(participant)) participants.Add(participant);
        }

        List<Tag> tags = [];
        for (int i = 0; i < tagIds?.Count; i++)
        {
            var tag = await _tagRepository.GetByIdAsync(tagIds[i]);
            // TODO retourner l'id du tag qui pose probleme
            if (tag == null) return new Response<EventDto?>(success: false, errors: ["Tag not found."]);

            if (!tags.Contains(tag)) tags.Add(tag);
        }

        var eventToAdd = new Event(description, startDate, endDate, room, eventCategory, participants, tags, null, null, null, null)
        {
            Room = room,
            EventCategory = eventCategory
        };
        var canAdd = CanAddEvent(room, eventToAdd);
        // TODO faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event
        if (!canAdd) return new Response<EventDto?>(success: false, errors: ["TODO"]);
        
        await _eventRepository.CreateAsync(eventToAdd);

        return new Response<EventDto?>(success: true, content: GetEventDto(eventToAdd));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var eventToDelete = await _eventRepository.GetByIdAsync(id);
        if (eventToDelete == null) return new Response<string>(success: false, errors: ["Event not found."]);

        await _eventRepository.DeleteAsync(eventToDelete);
        return new Response<string>(success: true, content: "Event deleted successfully.");
    }

    public async Task<Response<EventDto?>> GetByIdAsync(string id)
    {
        var eventToGet = await _eventRepository.GetByIdWithRelationsAsync(id);
        if (eventToGet == null) return new Response<EventDto?>(success: false, errors: ["Event not found."]);

        return new Response<EventDto?>(success: true, content: GetEventDto(eventToGet));
    }

    public async Task<Response<EventDto?>> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription,
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? newTagIds)
    {
        var eventToUpdate = await _eventRepository.GetByIdWithRelationsAsync(id);
        if (eventToUpdate == null) return new Response<EventDto?>(success: false, errors: ["Event not found."]);

        if (newStartDate == null && newEndDate == null && newRoomId == null && newDescription == null &&
            newEventCategoryId == null && newParticipantIds == null && newTagIds == null)
            return new Response<EventDto?>(success: false, errors: ["At least one field is required."]);

        if (newRoomId != null)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return new Response<EventDto?>(success: false, errors: ["Room not found."]);

            eventToUpdate.Room = room;
            eventToUpdate.RoomId = room.Id;
        }

        if (newEventCategoryId != null)
        {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (eventCategory == null) return new Response<EventDto?>(success: false, errors: ["Event category not found."]);

            eventToUpdate.EventCategory = eventCategory;
            eventToUpdate.EventCategoryId = eventCategory.Id;
        }

        List<Participant> participants = [];
        for (int i = 0; i < newParticipantIds?.Count; i++)
        {
            var participantToAdd = eventToUpdate.Participants.Find(x => x.Id == newParticipantIds[i]);
            if (participantToAdd == null)
            {
                var participant = await _participantRepository.GetByIdAsync(newParticipantIds[i]);
                if (participant == null) return new Response<EventDto?>(success: false, errors: ["Participant not found."]);
                participants.Add(participant);
            }
            else
            {
                participants.Add(participantToAdd);
            }
        }

        List<Tag> tags = [];
        for (int i = 0; i < newTagIds?.Count; i++)
        {
            var tagToAdd = eventToUpdate.Tags.Find(x => x.Id == newTagIds[i]);
            if (tagToAdd == null)
            {
                var tag = await _tagRepository.GetByIdAsync(newTagIds[i]);
                if (tag == null) return new Response<EventDto?>(success: false, errors: ["Tag not found."]);
                tags.Add(tag);
            }
            else
            {
                tags.Add(tagToAdd);
            }
        }

        eventToUpdate.StartDate = newStartDate ?? eventToUpdate.StartDate;
        eventToUpdate.EndDate = newEndDate ?? eventToUpdate.EndDate;
        eventToUpdate.Description = newDescription ?? eventToUpdate.Description;
        eventToUpdate.Participants = participants.Count > 0 ? participants : eventToUpdate.Participants;
        eventToUpdate.Tags = tags.Count > 0 ? tags : eventToUpdate.Tags;

        await _eventRepository.UpdateAsync(eventToUpdate);

        return new Response<EventDto?>(success: true, content: GetEventDto(eventToUpdate));
    }

    private static EventDto GetEventDto(Event baseEvent)
    {
        return new EventDto(baseEvent, new RoomDto(baseEvent.Room), new EventCategoryDto(baseEvent.EventCategory),
            baseEvent.Participants.Select(participant => new ParticipantDto(participant)).ToList(), baseEvent.Tags.Select(tag => new TagDto(tag)).ToList());
    }
    // TODO renvoyer un message d'erreur avec le slot ou l'event qui pose probleme
    private static bool CanAddEvent(Room room, Event @event)
    {
        var eventStartDate = new DateOnly(@event.StartDate.Year, @event.StartDate.Month, @event.StartDate.Day);
        var eventEndDate = new DateOnly(@event.EndDate.Year, @event.EndDate.Month, @event.EndDate.Day);
        
        var eventStartTime = new TimeOnly(@event.StartDate.Hour, @event.StartDate.Minute, @event.StartDate.Second);
        var eventEndTime = new TimeOnly(@event.EndDate.Hour, @event.EndDate.Minute, @event.EndDate.Second);  

        var roomSlots = room.Slots.Where(existingSlot => existingSlot.DeletedAt == null && 
            (existingSlot.StartDate <= eventStartDate && existingSlot.EndDate >= eventEndDate || 

            existingSlot.StartDate > eventStartDate && existingSlot.EndDate < eventEndDate ||

            existingSlot.StartDate < eventStartDate && existingSlot.EndDate > eventStartDate && existingSlot.EndDate < eventEndDate ||

            existingSlot.EndDate < eventEndDate && existingSlot.StartDate > eventStartDate && existingSlot.StartDate < eventEndDate)).ToList();

        if (roomSlots.Count > 0) 
        {
            var roomSlotsHour = roomSlots.Where(existingSlot => existingSlot.DeletedAt == null && 
            (existingSlot.StartTime <= eventStartTime && existingSlot.EndTime >= eventEndTime || 

            existingSlot.StartTime > eventStartTime && existingSlot.EndTime < eventEndTime ||

            existingSlot.StartTime < eventStartTime && existingSlot.EndTime > eventStartTime && existingSlot.EndTime < eventEndTime ||

            existingSlot.EndTime < eventEndTime && existingSlot.StartTime > eventStartTime && existingSlot.StartTime < eventEndTime)).ToList();
            if (roomSlotsHour.Count > 0) 
            {
                foreach (var roomSlotHour in roomSlotsHour)
                {
                    if (roomSlotHour.EventCategories.Count == 0) return false;
                    else if (roomSlotHour.EventCategories.Count > 0 && !roomSlotHour.EventCategories.Contains(@event.EventCategory)) return false;
                }
            }
        }

        var roomEvents = room.Events.Where(existingEvent => existingEvent.DeletedAt == null && 
            (existingEvent.StartDate <= @event.StartDate && existingEvent.EndDate >= @event.EndDate || 

            existingEvent.StartDate > @event.StartDate && existingEvent.EndDate < @event.EndDate ||

            existingEvent.StartDate < @event.StartDate && existingEvent.EndDate > @event.StartDate && existingEvent.EndDate < @event.EndDate ||

            existingEvent.EndDate < @event.EndDate && existingEvent.StartDate > @event.StartDate && existingEvent.StartDate < @event.EndDate)).ToList();

        return roomEvents.Count <= 0;
        
    }

    public async Task<Response<List<EventDto>?>> CreateWithRepetitionAsync(CreateEventWithRepetitionRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null) return new Response<List<EventDto>?>(success: false, errors: ["Room not found."]);

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(request.EventCategoryId);
        if (eventCategory == null) return new Response<List<EventDto>?>(success: false, errors: ["Event category not found."]);

        List<Participant> participants = [];
        for (int i = 0; i < request.ParticipantIds?.Count; i++)
        {
            var participant = await _participantRepository.GetByIdAsync(request.ParticipantIds[i]);

            if (participant == null) return new Response<List<EventDto>?>(success: false, errors: ["Participant not found."]);

            if (!participants.Contains(participant)) participants.Add(participant);      
        }

        List<Tag> tags = [];
        for (int i = 0; i < request.TagIds?.Count; i++)
        {
            var tag = await _tagRepository.GetByIdAsync(request.TagIds[i]);

            if (tag == null) return new Response<List<EventDto>?>(success: false, errors: ["Tag not found."]);

            if (!tags.Contains(tag)) tags.Add(tag);
        }

        var mainEvent = new Event(request.Description, request.StartDate, request.EndDate, room, eventCategory, participants, tags, 
            request.RepetitionInterval, request.RepetitionNumber, null, request.RepetitionEndDate)
        {
            Room = room,
            EventCategory = eventCategory
        };

        var canAdd = CanAddEvent(room, mainEvent);
        // TODO changer le type de response de canAdd et récupérer l'erreur en dessous a la place de todo
        if (!canAdd) return new Response<List<EventDto>?>(success: false, errors: ["TODO"]);
        
        List<Event> events = [mainEvent];

        var repetitionStartDate = _repetitionService.IncrementDateTime(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = _repetitionService.IncrementDateTime(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

        while (DateOnly.FromDateTime(repetitionStartDate) < request.RepetitionEndDate)
        {
            var @event = new Event(request.Description, repetitionStartDate, repetitionEndDate, room, eventCategory, participants, tags, 
                null, null, mainEvent, null)
            {
                Room = room,
                EventCategory = eventCategory
            };
            
            bool canAddEvent = CanAddEvent(room, @event);
            // TODO changer le type de response de canAdd et récupérer l'erreur en dessous a la place de todo
            if (!canAddEvent) return new Response<List<EventDto>?>(success: false, errors: ["TODO"]);

            events.Add(@event);
            
            repetitionStartDate = _repetitionService.IncrementDateTime(repetitionStartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = _repetitionService.IncrementDateTime(repetitionEndDate, request.RepetitionInterval, request.RepetitionNumber);
        }

        await _eventRepository.CreateMultipleAsync(events);

        return new Response<List<EventDto>?>(success: true, content: events.Select(x => GetEventDto(x)).ToList());
    }
}
