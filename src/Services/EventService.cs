using System.Text.Json;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using MongoDB.Bson;

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

    public async Task<EventDto?> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate,
        List<string>? participantIds, List<string>? tagIds)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return null;

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
        if (eventCategory == null) return null;

        List<Participant> participants = [];
        List<ParticipantDto> participantDtos = [];
        for (int i = 0; i < participantIds?.Count; i++)
        {
            var participant = await _participantRepository.GetByIdAsync(participantIds[i]);
            if (participant == null) return null;
            if (!participants.Contains(participant))
            {
                participants.Add(participant);
                participantDtos.Add(new ParticipantDto(participant));
            }
        }

        List<Tag> tags = [];
        List<TagDto> tagDtos = [];
        for (int i = 0; i < tagIds?.Count; i++)
        {
            var tag = await _tagRepository.GetByIdAsync(tagIds[i]);
            if (tag == null) return null;
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
                tagDtos.Add(new TagDto(tag));
            }
        }

        var eventToAdd = new Event(description, startDate, endDate, room, eventCategory, participants, tags)
        {
            Room = room,
            EventCategory = eventCategory
        };
        var canAdd = CanAddEvent(room, eventToAdd);
        if (!canAdd) return null;
        
        await _eventRepository.CreateAsync(eventToAdd);

        return GetEventDto(eventToAdd);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var eventToDelete = await _eventRepository.GetByIdAsync(id);
        if (eventToDelete == null) return false;

        await _eventRepository.DeleteAsync(eventToDelete);
        return true;
    }

    public async Task<EventDto?> GetByIdAsync(string id)
    {
        var eventToGet = await _eventRepository.GetByIdWithRelationsAsync(id);
        if (eventToGet == null) return null;

        return GetEventDto(eventToGet);
    }

    public async Task<EventDto?> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription,
        string? newEventCategoryId, List<string>? newParticipantIds, List<string>? newTagIds)
    {
        var eventToUpdate = await _eventRepository.GetByIdWithRelationsAsync(id);
        if (eventToUpdate == null || (newStartDate == null && newEndDate == null && newRoomId == null && newDescription == null &&
            newEventCategoryId == null && newParticipantIds == null && newTagIds == null)) return null;

        if (newRoomId != null)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return null;

            eventToUpdate.Room = room;
            eventToUpdate.RoomId = room.Id;
        }

        if (newEventCategoryId != null)
        {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (eventCategory == null) return null;

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
                if (participant == null) return null;
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
                if (tag == null) return null;
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

        return GetEventDto(eventToUpdate);
    }

    private static EventDto GetEventDto(Event baseEvent)
    {
        return new EventDto(baseEvent, new RoomDto(baseEvent.Room), new EventCategoryDto(baseEvent.EventCategory),
            baseEvent.Participants.Select(participant => new ParticipantDto(participant)).ToList(), baseEvent.Tags.Select(tag => new TagDto(tag)).ToList());
    }

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
                    if (roomSlotHour.EventCategories.Count > 0 && !roomSlotHour.EventCategories.Contains(@event.EventCategory))
                    {
                        Console.WriteLine(roomSlotHour.EventCategories.ToJson());
                        return false;
                    }
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
}
