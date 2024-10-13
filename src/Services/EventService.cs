using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using BachelorTherasoftDotnetApi.Utils;
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
    private readonly IMapper _mapper;
    public EventService(IEventRepository eventRepository, IEventCategoryRepository eventCategoryRepository, IRoomRepository roomRepository, IMapper mapper,
        IParticipantRepository participantRepository, ITagRepository tagRepository)
    {
        _eventRepository = eventRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _participantRepository = participantRepository;
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult<EventDto>> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate,
        List<string>? participantIds, List<string>? tagIds)
    {
        var res = await _roomRepository.GetEntityByIdAsync(roomId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(roomId, "Room");

        var res2 = await _eventCategoryRepository.GetEntityByIdAsync(eventCategoryId);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        if (res2.Data == null) return Response.NotFound(eventCategoryId, "Event category");

        List<Participant> participants = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < participantIds?.Count; i++)
        {
            var res3 = await _participantRepository.GetEntityByIdAsync(participantIds[i]);
            if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);
            if (res3.Data == null) return Response.NotFound(participantIds[i], "Participant");

            if (!participants.Contains(res3.Data)) participants.Add(res3.Data);
        }

        List<Tag> tags = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < tagIds?.Count; i++)
        {
            var res4 = await _tagRepository.GetEntityByIdAsync(tagIds[i]);
            if (!res4.Success) return Response.BadRequest(res4.Message, res4.Details);
            if (res4.Data == null) return Response.NotFound(tagIds[i],"Tag");

            if (!tags.Contains(res4.Data)) tags.Add(res4.Data);
        }

        var eventToAdd = new Event(description, startDate, endDate, res.Data, res2.Data, participants, tags, null, null, null, null)
        {
            Room = res.Data,
            EventCategory = res2.Data
        };
        var canAdd = CanAddEvent(res.Data, eventToAdd);
        // TODO faire en sorte de retourner si c'est un probleme de slot ou un probleme d'event
        if (!canAdd) return Response.BadRequest("Cant add event.", null);

        await _eventRepository.CreateAsync(eventToAdd);

        return Response.CreatedAt(_mapper.Map<EventDto>(eventToAdd));   
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _eventRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        
        return Response.NoContent();
    }

    public async Task<ActionResult<EventDto>> GetByIdAsync(string id)
    {
        var eventToGet = await _eventRepository.GetByIdAsync<EventDto>(id);
        if (eventToGet == null) return Response.NotFound(id, "Event");

        return Response.Ok(eventToGet);
    }

    public async Task<ActionResult<EventDto>> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription,
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? newTagIds)
    {
        if (newStartDate == null && newEndDate == null && newRoomId == null && newDescription == null && newEventCategoryId == null && newParticipantIds == null && newTagIds == null)
            return new BadRequestObjectResult(new { errors = new List<string>(["At least one field is required"]) });
            
        var eventToUpdate = await _eventRepository.GetByIdJoinRelationsAsync(id);
        if (eventToUpdate == null) return Response.NotFound(id, "Event");


        if (newRoomId != null)
        {
            var res = await _roomRepository.GetEntityByIdAsync(id);
            if (!res.Success) return Response.BadRequest(res.Message, res.Details);
            if (res.Data == null) return Response.NotFound(id, "Room");

            eventToUpdate.Room = res.Data;
            eventToUpdate.RoomId = res.Data.Id;
        }

        if (newEventCategoryId != null)
        {
            var res2 = await _eventCategoryRepository.GetEntityByIdAsync(id);
            if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
            if (res2.Data == null) return Response.NotFound(newEventCategoryId, "Event category");

            eventToUpdate.EventCategory = res2.Data;
            eventToUpdate.EventCategoryId = res2.Data.Id;
        }

        List<Participant> participants = [];
        // TODO a refactor comme le create
        for (int i = 0; i < newParticipantIds?.Count; i++)
        {
            var participantToAdd = eventToUpdate.Participants.Find(x => x.Id == newParticipantIds[i]);
            if (participantToAdd == null)
            {
                var res3 = await _participantRepository.GetEntityByIdAsync(newParticipantIds[i]);
                if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);
                if (res3.Data == null) return Response.NotFound(newParticipantIds[i], "Particpant");
                participants.Add(res3.Data);
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
                var res4 = await _tagRepository.GetEntityByIdAsync(newTagIds[i]);
                if (!res4.Success) return Response.BadRequest(res4.Message, res4.Details);
                if (res4.Data == null) return Response.NotFound(newTagIds[i], "Tag");
                tags.Add(res4.Data);
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

        return Response.Ok(_mapper.Map<EventDto>(eventToUpdate));
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
        var res = await _roomRepository.GetEntityByIdAsync(request.RoomId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(request.RoomId, "Room");

        var res2 = await _eventCategoryRepository.GetEntityByIdAsync(request.EventCategoryId);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        if (res2.Data == null) return Response.NotFound(request.EventCategoryId, "Event category");

        List<Participant> participants = [];
        // TODO refactor ceci en une requete et voir tous ceux qui manque / si il y a des doublons et les renvoyé avec l'erreur
        for (int i = 0; i < request.ParticipantIds?.Count; i++)
        {
            var res3 = await _participantRepository.GetEntityByIdAsync(request.ParticipantIds[i]);
            if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);
            if (res3.Data == null) return Response.NotFound(request.ParticipantIds[i], "Participant");

            if (!participants.Contains(res3.Data)) participants.Add(res3.Data);
        }

        List<Tag> tags = [];
        for (int i = 0; i < request.TagIds?.Count; i++)
        {
            var res4 = await _tagRepository.GetEntityByIdAsync(request.TagIds[i]);
            if (!res4.Success) return Response.BadRequest(res4.Message, res4.Details);
            if (res4.Data == null) return Response.NotFound(request.TagIds[i], "Tag");

            if (!tags.Contains(res4.Data)) tags.Add(res4.Data);
        }

        var mainEvent = new Event(request.Description, request.StartDate, request.EndDate, res.Data, res2.Data, participants, tags,
            request.RepetitionInterval, request.RepetitionNumber, null, request.RepetitionEndDate)
        {
            Room = res.Data,
            EventCategory = res2.Data
        };

        var canAdd = CanAddEvent(res.Data, mainEvent);
        // TODO changer le type de response de canAdd et récupérer l'erreur en dessous a la place de todo
        if (!canAdd) return new BadRequestObjectResult(new { errors = new List<string>(["Cant add event bcs of slot or event."])});

        List<Event> events = [mainEvent];

        var repetitionStartDate = Repetition.IncrementDateTime(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = Repetition.IncrementDateTime(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

        while (DateOnly.FromDateTime(repetitionStartDate) < request.RepetitionEndDate)
        {
            var @event = new Event(request.Description, repetitionStartDate, repetitionEndDate, res.Data, res2.Data, participants, tags,
                null, null, mainEvent, null)
            {
                Room = res.Data,
                EventCategory = res2.Data
            };

            bool canAddEvent = CanAddEvent(res.Data, @event);
            // TODO changer le type de response de canAdd et récupérer l'erreur en dessous a la place de todo
            if (!canAddEvent) return new BadRequestObjectResult(new { errors = new List<string>(["Cant add event bcs of slot or event."])});

            events.Add(@event);

            repetitionStartDate = Repetition.IncrementDateTime(repetitionStartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = Repetition.IncrementDateTime(repetitionEndDate, request.RepetitionInterval, request.RepetitionNumber);
        }

        await _eventRepository.CreateMultipleAsync(events);

        return Response.CreatedAt(events.Select(x => _mapper.Map<EventDto>(x)).ToList());  
    }
}
