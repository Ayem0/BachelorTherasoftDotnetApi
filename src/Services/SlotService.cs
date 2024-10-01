using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class SlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IRoomRepository _roomRepository;
    public SlotService(ISlotRepository slotRepository, IWorkspaceRepository workspaceRepository, IRoomRepository roomRepository)
    {
        _slotRepository = slotRepository;
        _workspaceRepository = workspaceRepository;
        _roomRepository = roomRepository;
    }

    public async Task<SlotDto?> GetByIdAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);

        return slot != null ? new SlotDto(slot) : null;
    }

    public async Task<SlotDto?> CreateAsync(string workspaceId, DateOnly startDate, DateOnly endDate) 
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var slot = new Slot(workspace, startDate, endDate) {
            Workspace = workspace
        };
        await _slotRepository.CreateAsync(slot);

        return new SlotDto(slot);
    }

    public async Task<SlotDto?> UpdateAsync(string id, DateOnly? newStartDate, DateOnly? newEndDate) 
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) return null;

        slot.StartDate = newStartDate ?? slot.StartDate;
        slot.EndDate = newEndDate ?? slot.EndDate;    
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

    // public async Task<bool> AddSlotToRoom(string slotId, string roomId)
    // {
    //     var room = await _roomRepository.GetByIdAsync(roomId);
    //     if (room == null) return false;

    //     var slot = await _slotRepository.GetByIdAsync(slotId);
    //     if (slot == null) return false;

    //     var roomSlots = room.Slots.Where(roomSlot => roomSlot.DeletedAt == null && 
    //         (roomSlot.StartDate <= slot.StartDate && roomSlot.EndDate >= slot.EndDate || 
    //         roomSlot.StartDate > slot.StartDate && roomSlot.EndDate < slot.EndDate ||

    //         roomSlot.StartDate < slot.StartDate && roomSlot.EndDate > slot.StartDate && roomSlot.EndDate < slot.EndDate ||

    //         roomSlot.EndDate < slot.EndDate && roomSlot.StartDate > slot.StartDate && roomSlot.StartDate < slot.EndDate )).ToList();
        
    //     string[,] RoomSlotMatrix = new string[288, 7];
    //     for ( int j = 0; j < 7; j++) {
    //         for (int i = 0; i < 288; i++ ) {
                
    //         }
    //     }



    // }
}
