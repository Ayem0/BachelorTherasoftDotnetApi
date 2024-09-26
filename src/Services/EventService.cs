using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Repositories;

namespace BachelorTherasoftDotnetApi.src.Services;

// TODO Ajouter les tags et les users des events
// TODO Ajouter une fonctione de vérification pour check si la date est disponible a la création et a la modification

public class EventService : IEventService
{
    private readonly EventRepository _eventRepository;
    private readonly RoomRepository _roomRepository;
    private readonly EventCategoryRepository _eventCategoryRepository;
    public EventService(EventRepository eventRepository, EventCategoryRepository eventCategoryRepository, RoomRepository roomRepository)
    {
        _eventRepository = eventRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
    }

    public async Task<EventDto?> CreateAsync(string? description, string roomId, string eventCategoryId, DateTime startDate, DateTime endDate)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return null;

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
        if (eventCategory == null) return null;

        var eventToAdd = new Event {
            Description = description ?? null,
            StartDate = startDate,
            EndDate = endDate,
            Room = room,
            RoomId = room.Id,
            EventCategory = eventCategory,
            EventCategoryId = eventCategory.Id
            
        };
        
        await _eventRepository.CreateAsync(eventToAdd);

        var eventDto = new EventDto {
            EndDate = eventToAdd.EndDate,
            Id = eventToAdd.Id,
            StartDate = eventToAdd.StartDate,
            Room = new RoomDto {
                Id = eventToAdd.Room.Id,
                Name = eventToAdd.Room.Name
            },
            RoomId = eventToAdd.Room.Id,
            EventCategory = new EventCategoryDto {
                Id = eventToAdd.EventCategory.Icon,
                Name = eventToAdd.EventCategory.Name,
                Icon = eventToAdd.EventCategory.Icon,
            },
            EventCategoryId = eventToAdd.EventCategory.Id
        };

        return eventDto;
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
        var eventToGet = await _eventRepository.GetByIdAsync(id);
        if (eventToGet == null) return null;
        
        var eventDto = new EventDto{
            EndDate = eventToGet.EndDate,
            StartDate = eventToGet.StartDate,
            Id = eventToGet.Id,
            Room = new RoomDto {
                Id = eventToGet.Room.Id,
                Name = eventToGet.Room.Name
            },
            RoomId = eventToGet.RoomId,
            EventCategory = new EventCategoryDto {
                Id = eventToGet.EventCategory.Icon,
                Name = eventToGet.EventCategory.Name,
                Icon = eventToGet.EventCategory.Icon,
            },
            EventCategoryId = eventToGet.EventCategory.Id
        };
        return eventDto;
    }

    public async Task<bool> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription, string? newEventCategoryId)
    {
        var eventToUpdate = await _eventRepository.GetByIdAsync(id);
        if (eventToUpdate == null) return false;

        EventCategory? eventCategory = null;
        if (newEventCategoryId != null) {
            eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if ( eventCategory == null) return false;
        }

        Room? room = null;
        if (newRoomId != null) {
            room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return false;
        }

        eventToUpdate.StartDate = newStartDate ?? eventToUpdate.StartDate;
        eventToUpdate.EndDate = newEndDate ?? eventToUpdate.EndDate;
        eventToUpdate.Description = newDescription ?? eventToUpdate.Description;
        eventToUpdate.EventCategory = eventCategory ?? eventToUpdate.EventCategory;
        eventToUpdate.EventCategoryId = eventCategory?.Id ?? eventToUpdate.EventCategoryId;
        eventToUpdate.Room = room ?? eventToUpdate.Room;
        eventToUpdate.RoomId = room?.Id ?? eventToUpdate.RoomId;

        await _eventRepository.UpdateAsync(eventToUpdate);
        return true;
    }
}
