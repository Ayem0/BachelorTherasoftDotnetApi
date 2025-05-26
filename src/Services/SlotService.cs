using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using BachelorTherasoftDotnetApi.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class SlotService : ISlotService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;

    public SlotService(
        ISlotRepository slotRepository,
        IWorkspaceRepository workspaceRepository,
        IEventCategoryRepository eventCategoryRepository,
        IMapper mapper,
        ISocketService socket
    )
    {
        _slotRepository = slotRepository;
        _workspaceRepository = workspaceRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _mapper = mapper;
        _socket = socket;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync(id) ?? throw new NotFoundException("Slot", id);
        var success = await _slotRepository.DeleteAsync(slot);
        if (success)
        {
            await _socket.NotififyGroup(slot.WorkspaceId, "SlotDeleted", id);
        }
        return success;
    }

    public async Task<SlotDto?> GetByIdAsync(string id)
    => _mapper.Map<SlotDto?>(await _slotRepository.GetByIdAsync(id));

    public async Task<List<SlotDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<SlotDto>>(await _slotRepository.GetByWorkpaceIdAsync(workspaceId));

    public async Task<SlotDto> CreateAsync(CreateSlotRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        List<EventCategory> eventCategories = [];
        if (req.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in req.EventCategoryIds)
            {
                var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId) ?? throw new NotFoundException("Event category", eventCategoryId);
                eventCategories.Add(eventCategory);
            }
        }

        var slot = new Slot(req.Name, req.Description, workspace, req.StartDate, req.EndDate, req.StartTime, req.EndTime, eventCategories, null, null, null, null)
        {
            Workspace = workspace
        };
        var created = await _slotRepository.CreateAsync(slot);
        var dto = _mapper.Map<SlotDto>(created);

        await _socket.NotififyGroup(created.WorkspaceId, "SlotCreated", dto);

        return dto;
    }

    public async Task<List<SlotDto>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        List<EventCategory> eventCategories = [];
        if (req.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in req.EventCategoryIds)
            {
                var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId) ?? throw new NotFoundException("Event category", eventCategoryId);
                eventCategories.Add(eventCategory);
            }
        }

        var mainSlot = new Slot(req.Name, req.Description, workspace, req.StartDate, req.EndDate, req.StartTime, req.EndTime, eventCategories,
        req.RepetitionInterval, req.RepetitionNumber, null, req.RepetitionEndDate)
        {
            Workspace = workspace
        };

        List<Slot> slots = [mainSlot];
        var repetitionStartDate = Repetition.IncrementDateOnly(req.StartDate, req.RepetitionInterval, req.RepetitionNumber);
        var repetitionEndDate = Repetition.IncrementDateOnly(req.EndDate, req.RepetitionInterval, req.RepetitionNumber);

        while (repetitionStartDate < req.RepetitionEndDate)
        {
            var slot = new Slot(req.Name, req.Description, workspace, repetitionStartDate, repetitionEndDate, req.StartTime, req.EndTime, eventCategories, null, null, mainSlot, null)
            {
                Workspace = workspace
            };
            slots.Add(slot);

            repetitionStartDate = Repetition.IncrementDateOnly(req.StartDate, req.RepetitionInterval, req.RepetitionNumber);
            repetitionEndDate = Repetition.IncrementDateOnly(req.EndDate, req.RepetitionInterval, req.RepetitionNumber);
        }
        var created = await _slotRepository.CreateMultipleAsync(slots);
        var dto = _mapper.Map<List<SlotDto>>(created);

        await _socket.NotififyGroup(req.WorkspaceId, "SlotsCreated", dto);

        return dto;
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

    // // TODO voir si faire AddRoomToSlot plutot
    // public async Task<bool> AddSlotToRoom(string slotId, string roomId)
    // {
    //     var room = await _roomRepository.GetJoinEventsSlotsByRangeAndIdAsync(roomId) ?? throw new NotFoundException("Room", roomId);

    //     var slot = await _slotRepository.GetByIdAsync(slotId) ?? throw new NotFoundException("Slot", slotId);

    //     List<Slot> slots = [slot];

    //     if (slot.RepetitionEndDate != null && slot.RepetitionInterval != null && slot.RepetitionNumber != null && slot.MainSlotId != null)
    //     {
    //         var repetitionsSlots = await _slotRepository.GetRepetitionsById(slotId);
    //         slots.AddRange(repetitionsSlots);
    //     }

    //     foreach (var slotToAdd in slots)
    //     {
    //         var canAdd = CanAddSlotToRoom(room, slotToAdd);
    //         if (canAdd != null) return false;
    //     }

    //     room.Slots.AddRange(slots);
    //     await _roomRepository.UpdateAsync(room);

    //     return true;
    // }

    // private static List<Slot>? CanAddSlotToRoom(Room room, Slot slot)
    // {
    //     // TODO si garder ce fonctionnement refactoriser cette partie et return quel slot ou evenement pose probleme
    //     var roomSlots = room.Slots.Where(existingSlot => existingSlot.DeletedAt == null &&
    //         (existingSlot.StartDate <= slot.StartDate && existingSlot.EndDate >= slot.EndDate && existingSlot.StartTime <= slot.StartTime && existingSlot.EndTime >= slot.EndTime ||

    //         existingSlot.StartDate > slot.StartDate && existingSlot.EndDate < slot.EndDate && existingSlot.StartTime > slot.StartTime && existingSlot.EndTime < slot.EndTime ||

    //         existingSlot.StartDate < slot.StartDate && existingSlot.EndDate > slot.StartDate && existingSlot.EndDate < slot.EndDate && existingSlot.StartTime < slot.StartTime && existingSlot.EndTime > slot.StartTime && existingSlot.EndTime < slot.EndTime ||

    //         existingSlot.EndDate < slot.EndDate && existingSlot.StartDate > slot.StartDate && existingSlot.StartDate < slot.EndDate && existingSlot.EndTime < slot.EndTime && existingSlot.StartTime > slot.StartTime && existingSlot.StartTime < slot.EndTime)).ToList();

    //     if (roomSlots.Count != 0) return roomSlots;
    //     // TODO faire le check si il y a déja des rendez vous entre les dates et qui n'ont pas au moins une des catégories
    //     return null;
    // }


}
