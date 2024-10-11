using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<SlotDto>> GetByIdAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) return new NotFoundObjectResult("Slot not found.");

        return new OkObjectResult(new SlotDto(slot));
    }

    public async Task<ActionResult<SlotDto>> CreateAsync(CreateSlotRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds)
            {
                var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);

                // TODO return la catégorie qui fail actuellement je peux pas car je n'ai pas le name de la catégorie
                if (eventCategory == null) return new NotFoundObjectResult("Event category not found."); 

                eventCategories.Add(eventCategory);
            }
        }

        var slot = new Slot(workspace, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, null, null, null, null)
        {
            Workspace = workspace
        };
        await _slotRepository.CreateAsync(slot);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Slot", 
            routeValues: new { id = slot.Id }, 
            value: new SlotDto(slot)
        );  
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

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        if (slot == null) return new NotFoundObjectResult("Slot not found.");

        await _slotRepository.DeleteAsync(slot);

       return new OkObjectResult("Successfully deleted slot.");
    }
    // TODO voir si faire AddRoomToSlot plutot
    public async Task<ActionResult> AddSlotToRoom(string slotId, string roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room == null) return new NotFoundObjectResult(new { Error = "Room not found."});

        var slot = await _slotRepository.GetByIdAsync(slotId);
        if (slot == null) return new NotFoundObjectResult("Slot not found.");

        List<Slot> slots = [slot];

        if (slot.RepetitionEndDate != null && slot.RepetitionInterval != null && slot.RepetitionNumber != null && slot.MainSlotId != null)
        {
            var repetitionsSlots = await _slotRepository.GetRepetitionsById(slotId);
            slots.AddRange(repetitionsSlots);
        }

        foreach (var slotToAdd in slots)
        {
            var canAdd = CanAddSlotToRoom(room, slotToAdd);
            if (canAdd != null) return new BadRequestObjectResult(new { Error = "Slot is not compatible with existing slots.", Slots = canAdd.Select(x => new SlotDto(x)).ToList()});
        }

        room.Slots.AddRange(slots);
        await _roomRepository.UpdateAsync(room);
            
        return new OkObjectResult(new { Message = "Successfully added slot to room."});
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

    public async Task<ActionResult<List<SlotDto>>> CreateWithRepetitionAsync(CreateSlotWithRepetitionRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new NotFoundObjectResult(new { Error = "Workspace not found."});

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds)
            {
                var eventCategory = await _eventCategoryRepository.GetByIdAsync(eventCategoryId);
                if (eventCategory == null) return new NotFoundObjectResult(new { Error = "Event category not found.", EventCategoryId = eventCategoryId}); // TODO ajouter l'id de la catégorie
                eventCategories.Add(eventCategory);
            }
        }

        var mainSlot = new Slot(workspace, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, request.RepetitionInterval, request.RepetitionNumber,
            null, request.RepetitionEndDate)
        {
            Workspace = workspace
        };

        List<Slot> slots = [mainSlot];
        var repetitionStartDate = StaticRepetitionService.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = StaticRepetitionService.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

        while (repetitionStartDate < request.RepetitionEndDate)
        {
            var slot = new Slot(workspace, repetitionStartDate, repetitionEndDate, request.StartTime, request.EndTime, eventCategories, null, null, mainSlot, null)
            {
                Workspace = workspace
            };
            slots.Add(slot);

            repetitionStartDate = StaticRepetitionService.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = StaticRepetitionService.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);
        }
        await _slotRepository.CreateMultipleAsync(slots);

        return new CreatedAtActionResult(
            actionName: "CreateWithRepetition", 
            controllerName: "Slot", 
            routeValues: null, 
            value: slots.Select(x => new SlotDto(x)).ToList()
        );  
    }
}
