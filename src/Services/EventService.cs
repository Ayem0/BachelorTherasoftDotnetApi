using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    public EventService(IEventRepository eventRepository, IEventCategoryRepository eventCategoryRepository, IRoomRepository roomRepository)
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
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            Room = room,
            RoomId = room.Id,
            EventCategory = eventCategory,
            EventCategoryId = eventCategory.Id
        };
        
        await _eventRepository.CreateAsync(eventToAdd);

        var eventDto = new EventDto {
            Id = eventToAdd.Id,
            Description = description,
            StartDate = eventToAdd.StartDate,
            EndDate = eventToAdd.EndDate,
            Room = new RoomDto {
                Id = eventToAdd.Room.Id,
                Name = eventToAdd.Room.Name
            },
            EventCategory = new EventCategoryDto {
                Id = eventToAdd.EventCategory.Id,
                Name = eventToAdd.EventCategory.Name,
                Icon = eventToAdd.EventCategory.Icon,
            }
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
        
        var eventDto = new EventDto {
            EndDate = eventToGet.EndDate,
            StartDate = eventToGet.StartDate,
            Id = eventToGet.Id,
            Room = new RoomDto {
                Id = eventToGet.Room.Id,
                Name = eventToGet.Room.Name
            },
            EventCategory = new EventCategoryDto {
                Id = eventToGet.EventCategory.Icon,
                Name = eventToGet.EventCategory.Name,
                Icon = eventToGet.EventCategory.Icon,
            }
        };

        return eventDto;
    }

    public async Task<bool> UpdateAsync(string id, DateTime? newStartDate, DateTime? newEndDate, string? newRoomId, string? newDescription, string? newEventCategoryId)
    {
        var eventToUpdate = await _eventRepository.GetByIdAsync(id);
        if (eventToUpdate == null || (newStartDate == null && newEndDate == null && newRoomId == null && newDescription == null && newEventCategoryId == null)) return false;

        if (newRoomId != null) {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return false;

            eventToUpdate.Room = room;
            eventToUpdate.RoomId = room.Id;
        }

        if (newEventCategoryId != null) {
            var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (eventCategory == null) return false;

            eventToUpdate.EventCategory = eventCategory;
            eventToUpdate.EventCategoryId = eventCategory.Id;
        }

        eventToUpdate.StartDate = newStartDate ?? eventToUpdate.StartDate;
        eventToUpdate.EndDate = newEndDate ?? eventToUpdate.EndDate;
        eventToUpdate.Description = newDescription ?? eventToUpdate.Description;

        await _eventRepository.UpdateAsync(eventToUpdate);
        
        return true;
    }
}
