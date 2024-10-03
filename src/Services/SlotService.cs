using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class SlotService : ISlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IRoomRepository _roomRepository;
    public SlotService(ISlotRepository slotRepository, IWorkspaceRepository workspaceRepository, IRoomRepository roomRepository, IEventCategoryRepository eventCategoryRepository)
    {
        _slotRepository = slotRepository;
        _workspaceRepository = workspaceRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
    }

    public async Task<SlotDto?> GetByIdAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);

        return slot != null ? new SlotDto(slot) : null;
    }

    public async Task<SlotDto?> CreateAsync(string workspaceId, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, List<string>? eventCategoryIds) 
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        List<EventCategory> eventCategories = [];
        if (eventCategoryIds != null)
        {
            foreach(var eventCategoryId in eventCategoryIds)
            {   
                var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
                if (eventCategory == null) return null;
                eventCategories.Add(eventCategory);
            }
        }

        var slot = new Slot(workspace, startDate, endDate, startTime, endTime, eventCategories) {
            Workspace = workspace
        };
        await _slotRepository.CreateAsync(slot);

        return new SlotDto(slot);
    }

    public async Task<SlotDto?> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate, TimeOnly? newStartTime, TimeOnly? newEndTime) 
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) return null;

        slot.StartDate = newStartDate ?? slot.StartDate;
        slot.EndDate = newEndDate ?? slot.EndDate;  
        slot.StartTime = newStartTime ?? slot.StartTime;
        slot.EndTime = newEndTime ?? slot.EndTime;      
        await _slotRepository.UpdateAsync(slot);

        return new SlotDto(slot);
    }

    public async Task<bool> DeleteAsync(string id) 
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) return false;

        await _slotRepository.DeleteAsync(slot);

        return true;
    }

    public async Task<bool> AddSlotToRoom(string slotId, string roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return false;

        var slot = await _slotRepository.GetByIdAsync(slotId);
        if (slot == null) return false;

       var canAdd = CanAddSlotToRoom(room, slot);

       if (canAdd)
       {
            room.Slots.Add(slot);
            await _roomRepository.UpdateAsync(room);
       }
       return canAdd;
    }

    private static bool CanAddSlotToRoom(Room room, Slot slot)
    {
        // TODO si garder ce fonctionnement refactoriser cette partie
        var roomSlots = room.Slots.Where(existingSlot => existingSlot.DeletedAt == null && 
            (existingSlot.StartDate <= slot.StartDate && existingSlot.EndDate >= slot.EndDate || 

            existingSlot.StartDate > slot.StartDate && existingSlot.EndDate < slot.EndDate ||

            existingSlot.StartDate < slot.StartDate && existingSlot.EndDate > slot.StartDate && existingSlot.EndDate < slot.EndDate ||

            existingSlot.EndDate < slot.EndDate && existingSlot.StartDate > slot.StartDate && existingSlot.StartDate < slot.EndDate)).ToList();

        if (roomSlots.Count == 0) return true;
        var roomSlotHours = roomSlots.Where(existingSlot => existingSlot.DeletedAt == null && 
            (existingSlot.StartTime <= slot.StartTime && existingSlot.EndTime >= slot.EndTime || 

            existingSlot.StartTime > slot.StartTime && existingSlot.EndTime < slot.EndTime ||

            existingSlot.StartTime < slot.StartTime && existingSlot.EndTime > slot.StartTime && existingSlot.EndTime < slot.EndTime ||

            existingSlot.EndTime < slot.EndTime && existingSlot.StartTime > slot.StartTime && existingSlot.StartTime < slot.EndTime)).ToList();
        if (roomSlotHours.Count == 0) return true;
        // TODO faire le check si il y a déja des rendez vous entre les dates et qui n'ont pas au moins une des catégories
        return false;
    }
}
