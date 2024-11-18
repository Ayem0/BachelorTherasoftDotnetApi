using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class SlotService : ISlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public SlotService(ISlotRepository slotRepository, IWorkspaceRepository workspaceRepository, IRoomRepository roomRepository, IEventCategoryRepository eventCategoryRepository, IMapper mapper)
    {
        _slotRepository = slotRepository;
        _workspaceRepository = workspaceRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _mapper = mapper;
    }

    public async Task<SlotDto> GetByIdAsync(string id)
    {
        var slot = await _slotRepository.GetEntityByIdAsync(id)?? throw new NotFoundException("Slot", id);

        return _mapper.Map<SlotDto>(slot);
    }

    public async Task<SlotDto> CreateAsync(CreateSlotRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId) ?? throw new NotFoundException("Workspace", request.WorkspaceId);

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds)
            {
                var eventCategory = await _eventCategoryRepository.GetEntityByIdAsync(eventCategoryId) ?? throw new NotFoundException("EventCategory", eventCategoryId);
                eventCategories.Add(eventCategory);
            }
        }

        var slot = new Slot(workspace, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, null, null, null, null)
        {
            Workspace = workspace
        };

        await _slotRepository.CreateAsync(slot);
        
        return _mapper.Map<SlotDto>(slot);
    }

    // public async Task<SlotDto?> UpdateAsync(string id, UpdateSlotRequest request)
    // {
    //     var slot = await _slotRepository.GetByIdAsync(id);
    //     if (slot == null) return null;

    //     slot.StartDate = newStartDate ?? slot.StartDate;
    //     slot.EndDate = newEndDate ?? slot.EndDate;
    //     slot.StartTime = newStartTime ?? slot.StartTime;
    //     slot.EndTime = newEndTime ?? slot.EndTime;
    //     await _slotRepository.UpdateAsync(slot);

    //     return new SlotDto(slot);
    // }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _slotRepository.DeleteAsync(id);
    }
    // TODO voir si faire AddRoomToSlot plutot
    public async Task<bool> AddSlotToRoom(string slotId, string roomId)
    {
        var room = await _roomRepository.GetJoinEventsSlotsByIdAsync(roomId) ?? throw new NotFoundException("Room", roomId);

        var slot = await _slotRepository.GetEntityByIdAsync(slotId) ?? throw new NotFoundException("Slot", slotId);

        List<Slot> slots = [slot];

        if (slot.RepetitionEndDate != null && slot.RepetitionInterval != null && slot.RepetitionNumber != null && slot.MainSlotId != null)
        {
            var repetitionsSlots = await _slotRepository.GetRepetitionsById(slotId);
            slots.AddRange(repetitionsSlots);
        }

        foreach (var slotToAdd in slots)
        {
            var canAdd = CanAddSlotToRoom(room, slotToAdd);
            if (canAdd != null) return false;
        }

        room.Slots.AddRange(slots);
        await _roomRepository.UpdateAsync(room);
            
        return true;
    }

    private static List<Slot>? CanAddSlotToRoom(Room room, Slot slot)
    {
        // TODO si garder ce fonctionnement refactoriser cette partie et return qu'elle slot ou evenement pose probleme
        var roomSlots = room.Slots.Where(existingSlot => existingSlot.DeletedAt == null &&
            (existingSlot.StartDate <= slot.StartDate && existingSlot.EndDate >= slot.EndDate && existingSlot.StartTime <= slot.StartTime && existingSlot.EndTime >= slot.EndTime ||

            existingSlot.StartDate > slot.StartDate && existingSlot.EndDate < slot.EndDate && existingSlot.StartTime > slot.StartTime && existingSlot.EndTime < slot.EndTime ||

            existingSlot.StartDate < slot.StartDate && existingSlot.EndDate > slot.StartDate && existingSlot.EndDate < slot.EndDate  && existingSlot.StartTime < slot.StartTime && existingSlot.EndTime > slot.StartTime && existingSlot.EndTime < slot.EndTime ||

            existingSlot.EndDate < slot.EndDate && existingSlot.StartDate > slot.StartDate && existingSlot.StartDate < slot.EndDate && existingSlot.EndTime < slot.EndTime && existingSlot.StartTime > slot.StartTime && existingSlot.StartTime < slot.EndTime)).ToList();

        if (roomSlots.Count != 0) return roomSlots;
        // TODO faire le check si il y a déja des rendez vous entre les dates et qui n'ont pas au moins une des catégories
        return null;
    }

    public async Task<List<SlotDto>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId) ?? throw new NotFoundException("Workspace", request.WorkspaceId);

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds) // TODO refactor
            {
                var eventCategory = await _eventCategoryRepository.GetEntityByIdAsync(eventCategoryId) ?? throw new NotFoundException("EventCategory", eventCategoryId);
                
                eventCategories.Add(eventCategory);
            }
        }

        var mainSlot = new Slot(workspace, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, request.RepetitionInterval, request.RepetitionNumber,
            null, request.RepetitionEndDate)
        {
            Workspace = workspace
        };

        List<Slot> slots = [mainSlot];
        var repetitionStartDate = Repetition.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = Repetition.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

        while (repetitionStartDate < request.RepetitionEndDate)
        {
            var slot = new Slot(workspace, repetitionStartDate, repetitionEndDate, request.StartTime, request.EndTime, eventCategories, null, null, mainSlot, null)
            {
                Workspace = workspace
            };
            slots.Add(slot);

            repetitionStartDate = Repetition.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = Repetition.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);
        }
        await _slotRepository.CreateMultipleAsync(slots);

        return slots.Select(x => _mapper.Map<SlotDto>(x)).ToList();  
    }
}
