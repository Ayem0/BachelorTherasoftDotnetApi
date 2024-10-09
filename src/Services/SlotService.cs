using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
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
    private readonly IRepetitionService _repetitionService;
    public SlotService(ISlotRepository slotRepository, IWorkspaceRepository workspaceRepository, IRoomRepository roomRepository, IEventCategoryRepository eventCategoryRepository,
        IRepetitionService repetitionService)
    {
        _slotRepository = slotRepository;
        _workspaceRepository = workspaceRepository;
        _roomRepository = roomRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _repetitionService = repetitionService;
    }

    public async Task<Response<SlotDto?>> GetByIdAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) return new Response<SlotDto?>(success: false, errors: ["Slot not found."]);

        return new Response<SlotDto?>(success: true, content: new SlotDto(slot));
    }

    public async Task<Response<SlotDto?>> CreateAsync(CreateSlotRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new Response<SlotDto?>(success: false, errors: ["Workspace not found."]);

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds)
            {
                var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
                if (eventCategory == null) return new Response<SlotDto?>(success: false, errors: ["Event category not found."]); // TODO ajouter l'id de la catégorie qui fail
                eventCategories.Add(eventCategory);
            }
        }

        var slot = new Slot(workspace, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, null, null, null, null)
        {
            Workspace = workspace
        };
        await _slotRepository.CreateAsync(slot);

        return new Response<SlotDto?>(success: true, content: new SlotDto(slot));
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

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) return new Response<string>(success: false, errors: ["Slot not found."]);

        await _slotRepository.DeleteAsync(slot);

       return new Response<string>(success: true, content: "Slot successfully deleted.");
    }
    // TODO voir si faire AddRoomToSlot plutot
    public async Task<Response<string>> AddSlotToRoom(string slotId, string roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return new Response<string>(success: false, errors: ["Room not found."]);

        var slot = await _slotRepository.GetByIdAsync(slotId);
        if (slot == null) return new Response<string>(success: false, errors: ["Slot not found."]);

        List<Slot> slots = [slot];

        if (slot.RepetitionEndDate != null && slot.RepetitionInterval != null && slot.RepetitionNumber != null && slot.MainSlotId != null)
        {
            var repetitionsSlots = await _slotRepository.GetRepetitionsById(slotId);
            slots.AddRange(repetitionsSlots);
        }

        foreach (var slotToAdd in slots)
        {
            var canAdd = CanAddSlotToRoom(room, slotToAdd);
            if (!canAdd) return new Response<string>(success: false, errors: ["TODO"]);
        }

        room.Slots.AddRange(slots);
        await _roomRepository.UpdateAsync(room);
            
        return new Response<string>(success: true, errors: ["Slot successfully added to room."]);
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

    public async Task<Response<List<SlotDto>?>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new Response<List<SlotDto>?>(success: false, errors: ["Workspace not found."]);

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds)
            {
                var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
                if (eventCategory == null) return new Response<List<SlotDto>?>(success: false, errors: ["Event category not found."]); // TODO ajouter l'id de la catégorie
                eventCategories.Add(eventCategory);
            }
        }

        var mainSlot = new Slot(workspace, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, request.RepetitionInterval, request.RepetitionNumber,
            null, request.RepetitionEndDate)
        {
            Workspace = workspace
        };

        List<Slot> slots = [mainSlot];
        var repetitionStartDate = _repetitionService.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = _repetitionService.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

        while (repetitionStartDate < request.RepetitionEndDate)
        {
            var slot = new Slot(workspace, repetitionStartDate, repetitionEndDate, request.StartTime, request.EndTime, eventCategories, null, null, mainSlot, null)
            {
                Workspace = workspace
            };
            slots.Add(slot);

            repetitionStartDate = _repetitionService.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = _repetitionService.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);
        }
        await _slotRepository.CreateMultipleAsync(slots);

        return new Response<List<SlotDto>?>(success: true, content: slots.Select(x => new SlotDto(x)).ToList());
    }
}
