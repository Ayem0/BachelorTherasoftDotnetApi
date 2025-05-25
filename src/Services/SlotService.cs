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
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);


    public SlotService(
        ISlotRepository slotRepository,
        IWorkspaceRepository workspaceRepository,
        IRoomRepository roomRepository,
        IEventCategoryRepository eventCategoryRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService socket
    )
    {
        _slotRepository = slotRepository;
        _workspaceRepository = workspaceRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _mapper = mapper;
        _cache = cache;
        _socket = socket;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.Slot(workspaceId, id);
        var slot = await _cache.GetOrSetAsync(key, () => _slotRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Slot", id);

        var success = await _slotRepository.DeleteAsync(slot);
        if (success)
        {
            await _socket.NotififyGroup(slot.WorkspaceId, "SlotDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.Slots(workspaceId),
                CacheKeys.Slot(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<SlotDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<Slot?, SlotDto?>(
        CacheKeys.Slot(workspaceId, id),
        () => _slotRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<SlotDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Slot>, List<SlotDto>>(
        CacheKeys.Slots(workspaceId),
        () => _slotRepository.GetByWorkpaceIdAsync(workspaceId),
        ttl
    );

    public async Task<SlotDto> CreateAsync(string workspaceId, CreateSlotRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        List<EventCategory> eventCategories = [];
        if (req.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in req.EventCategoryIds)
            {
                var eventCategory = await _cache.GetOrSetAsync(
                   CacheKeys.EventCategory(workspaceId, eventCategoryId),
                   () => _eventCategoryRepository.GetByIdAsync(eventCategoryId),
                   ttl
               ) ?? throw new NotFoundException("Event category", eventCategoryId);
                eventCategories.Add(eventCategory);
            }
        }

        var slot = new Slot(req.Name, req.Description, workspace, req.StartDate, req.EndDate, req.StartTime, req.EndTime, eventCategories, null, null, null, null)
        {
            Workspace = workspace
        };
        var created = await _slotRepository.CreateAsync(slot);
        var dto = _mapper.Map<SlotDto>(slot);

        await _socket.NotififyGroup(workspaceId, "SlotCreated", dto);
        await _cache.DeleteAsync(CacheKeys.Slots(workspaceId));

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

    public async Task<List<SlotDto>> CreateWithRepetitionAsync(string workspaceId, CreateSlotWithRepetitionRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        List<EventCategory> eventCategories = [];
        if (req.EventCategoryIds != null)
        {

            foreach (var eventCategoryId in req.EventCategoryIds)
            {
                var eventCategory = await _cache.GetOrSetAsync(
                   CacheKeys.EventCategory(workspaceId, eventCategoryId),
                   () => _eventCategoryRepository.GetByIdAsync(eventCategoryId),
                   ttl
               ) ?? throw new NotFoundException("Event category", eventCategoryId);
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
        var dto = _mapper.Map<List<SlotDto>>(slots);

        await _socket.NotififyGroup(workspaceId, "SlotsCreated", dto);
        await _cache.DeleteAsync(CacheKeys.Slots(workspaceId));

        return dto;
    }
}
