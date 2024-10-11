using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly ITagRepository _tagRepository;
    public EventService(IEventRepository eventRepository, IEventCategoryRepository eventCategoryRepository, IRoomRepository roomRepository,
        IParticipantRepository participantRepository, ITagRepository tagRepository)
    {
        _eventRepository = eventRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _participantRepository = participantRepository;
        _tagRepository = tagRepository;
    }

    public async Task<ActionResult<EventDto>> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate,
        List<string>? participantIds, List<string>? tagIds)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null)
            return new NotFoundObjectResult(new
            {
                errors = new List<string>(["Room not found."]),
                content = roomId
            });

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
        if (eventCategory == null) return new NotFoundObjectResult(new { errors = new List<string>(["Event category not found."]), content = eventCategoryId });

        List<Participant> participants = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < participantIds?.Count; i++)
        {
            var participant = await _participantRepository.GetByIdAsync(participantIds[i]);
            if (participant == null)
                return new NotFoundObjectResult(new
                {
                    errors = new List<string>(["Participant not found."]),
                    content = participantIds[i]
                });

            if (!participants.Contains(participant)) participants.Add(participant);
        }

        List<Tag> tags = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < tagIds?.Count; i++)
        {
            var tag = await _tagRepository.GetByIdAsync(tagIds[i]);
            if (tag == null) return new NotFoundObjectResult(new { errors = new List<string>(["Tag not found."]), content = tagIds[i] });

            if (!tags.Contains(tag)) tags.Add(tag);
        }

        var eventToAdd = new Event(description, startDate, endDate, room, eventCategory, participants, tags, null, null, null, null)
        {
            Room = room,
            EventCategory = eventCategory
        };
        var canAdd = CanAddEvent(room, eventToAdd);
        // TODO faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event
        if (!canAdd) return new NotFoundObjectResult(new { errors = new List<string>(["Cant add event."]) });

        await _eventRepository.CreateAsync(eventToAdd);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Event", 
            routeValues: new { id = eventToAdd.Id }, 
            value: GetEventDto(eventToAdd)
        );  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var eventToDelete = await _eventRepository.GetByIdAsync(id);
        if (eventToDelete == null) return new NotFoundObjectResult(new { errors = new List<string>(["Event not found."]), content = id });

        await _eventRepository.DeleteAsync(eventToDelete);
        // TODO modifier pour notifier tous les membres de l'event sauf celui qui l'a créé
        return new OkObjectResult(new { message = "Successfully deleted event."});
    }

    public async Task<ActionResult<EventDto>> GetByIdAsync(string id)
    {
        var eventToGet = await _eventRepository.GetByIdWithRelationsAsync(id);
        if (eventToGet == null) return new NotFoundObjectResult(new { errors = new List<string>(["Event not found."]), content = id });

        return new  OkObjectResult(GetEventDto(eventToGet));
    }

    public async Task<ActionResult<EventDto>> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription,
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? newTagIds)
    {
        if (newStartDate == null && newEndDate == null && newRoomId == null && newDescription == null && newEventCategoryId == null && newParticipantIds == null && newTagIds == null)
            return new BadRequestObjectResult(new { errors = new List<string>(["At least one field is required not found."]) });
            
        var eventToUpdate = await _eventRepository.GetByIdWithRelationsAsync(id);
        if (eventToUpdate == null) return new NotFoundObjectResult(new { errors = new List<string>(["Event not found."]), content = id });


        if (newRoomId != null)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return new NotFoundObjectResult(new { errors = new List<string>(["Room not found."]), content = newRoomId });

            eventToUpdate.Room = room;
            eventToUpdate.RoomId = room.Id;
        }

        if (newEventCategoryId != null)
        {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (eventCategory == null) return new NotFoundObjectResult(new { errors = new List<string>(["Event category not found."]), content = newEventCategoryId });

            eventToUpdate.EventCategory = eventCategory;
            eventToUpdate.EventCategoryId = eventCategory.Id;
        }

        List<Participant> participants = [];
        // TODO a refactor comme le create
        for (int i = 0; i < newParticipantIds?.Count; i++)
        {
            var participantToAdd = eventToUpdate.Participants.Find(x => x.Id == newParticipantIds[i]);
            if (participantToAdd == null)
            {
                var participant = await _participantRepository.GetByIdAsync(newParticipantIds[i]);
                if (participant == null) return new NotFoundObjectResult(new { errors = new List<string>(["Particpant not found."]), content = newParticipantIds[i] });
                participants.Add(participant);
            }
            else
            {
                participants.Add(participantToAdd);
            }
        }

        List<Tag> tags = [];
        // TODO a refactor comme le create
        for (int i = 0; i < newTagIds?.Count; i++)
        {
            var tagToAdd = eventToUpdate.Tags.Find(x => x.Id == newTagIds[i]);
            if (tagToAdd == null)
            {
                var tag = await _tagRepository.GetByIdAsync(newTagIds[i]);
                if (tag == null) return new NotFoundObjectResult(new { errors = new List<string>(["Tag not found."]), content = newTagIds[i] });
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

        return new OkObjectResult(GetEventDto(eventToUpdate));
    }

    private static EventDto GetEventDto(Event baseEvent)
    {
        return new EventDto(baseEvent, new RoomDto(baseEvent.Room), new EventCategoryDto(baseEvent.EventCategory),
            baseEvent.Participants.Select(participant => new ParticipantDto(participant)).ToList(), baseEvent.Tags.Select(tag => new TagDto(tag)).ToList());
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
            (existingEvent.StartDate <= @event.StartDate && existingEvent.EndDate >= @event.EndDate ||

            existingEvent.StartDate > @event.StartDate && existingEvent.EndDate < @event.EndDate ||

            existingEvent.StartDate < @event.StartDate && existingEvent.EndDate > @event.StartDate && existingEvent.EndDate < @event.EndDate ||

            existingEvent.EndDate < @event.EndDate && existingEvent.StartDate > @event.StartDate && existingEvent.StartDate < @event.EndDate)).ToList();

        return roomEvents.Count <= 0;

    }

    public async Task<ActionResult<List<EventDto>>> CreateWithRepetitionAsync(CreateEventWithRepetitionRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null) return new NotFoundObjectResult(new { errors = new List<string>(["Room not found."]), content = request.RoomId });

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(request.EventCategoryId);
        if (eventCategory == null) return new NotFoundObjectResult(new { errors = new List<string>(["Event category not found."]), content = request.EventCategoryId });

        List<Participant> participants = [];
        for (int i = 0; i < request.ParticipantIds?.Count; i++)
        {
            var participant = await _participantRepository.GetByIdAsync(request.ParticipantIds[i]);

            if (participant == null) return new NotFoundObjectResult(new { errors = new List<string>(["Participant not found."]), content = request.ParticipantIds[i] });

            if (!participants.Contains(participant)) participants.Add(participant);
        }

        List<Tag> tags = [];
        for (int i = 0; i < request.TagIds?.Count; i++)
        {
            var tag = await _tagRepository.GetByIdAsync(request.TagIds[i]);

            if (tag == null) return new NotFoundObjectResult(new { errors = new List<string>(["Tag not found."]), content = request.TagIds[i] });

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
        if (!canAdd) return new BadRequestObjectResult(new { errors = new List<string>(["Cant add event bcs of slot or event."])});

        List<Event> events = [mainEvent];

        var repetitionStartDate = StaticRepetitionService.IncrementDateTime(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = StaticRepetitionService.IncrementDateTime(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

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
            if (!canAddEvent) return new BadRequestObjectResult(new { errors = new List<string>(["Cant add event bcs of slot or event."])});

            events.Add(@event);

            repetitionStartDate = StaticRepetitionService.IncrementDateTime(repetitionStartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = StaticRepetitionService.IncrementDateTime(repetitionEndDate, request.RepetitionInterval, request.RepetitionNumber);
        }

        await _eventRepository.CreateMultipleAsync(events);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Event", 
            routeValues: new { id = mainEvent.Id }, 
            value: events.Select(x => GetEventDto(x)).ToList()
        );  
    }
}
