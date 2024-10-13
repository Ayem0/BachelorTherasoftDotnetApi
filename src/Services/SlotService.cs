using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using BachelorTherasoftDotnetApi.Utils;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<SlotDto>> GetByIdAsync(string id)
    {
        var slot = await _slotRepository.GetByIdAsync<SlotDto>(id);
        if (slot == null) return Response.NotFound(id, "Slot");

        return Response.Ok(slot);
    }

    public async Task<ActionResult<SlotDto>> CreateAsync(CreateSlotRequest request)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(request.WorkspaceId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(request.WorkspaceId, "Workspace");

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds)
            {
                var res2 = await _eventCategoryRepository.GetEntityByIdAsync(eventCategoryId);
                if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
                if (res2.Data == null) return Response.NotFound(eventCategoryId, "Event category"); 

                eventCategories.Add(res2.Data);
            }
        }

        var slot = new Slot(res.Data, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, null, null, null, null)
        {
            Workspace = res.Data
        };

        var res3 = await _slotRepository.CreateAsync(slot);
        if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);

        return Response.CreatedAt(_mapper.Map<SlotDto>(slot));
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
        var res = await _slotRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);

        return Response.Ok("Successfully deleted slot.");
    }
    // TODO voir si faire AddRoomToSlot plutot
    public async Task<ActionResult> AddSlotToRoom(string slotId, string roomId)
    {
        var res = await _roomRepository.GetEntityByIdAsync(roomId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(roomId, "Room");

        var res2 = await _slotRepository.GetEntityByIdAsync(slotId);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        if (res2.Data == null) return Response.NotFound(slotId, "Slot");

        List<Slot> slots = [res2.Data];

        if (res2.Data.RepetitionEndDate != null && res2.Data.RepetitionInterval != null && res2.Data.RepetitionNumber != null && res2.Data.MainSlotId != null)
        {
            var repetitionsSlots = await _slotRepository.GetRepetitionsById(slotId);
            slots.AddRange(repetitionsSlots);
        }

        foreach (var slotToAdd in slots)
        {
            var canAdd = CanAddSlotToRoom(res.Data, slotToAdd);
            if (canAdd != null) return Response.BadRequest("Slot is not compatible with existing slots.", string.Join(", ", canAdd.Select(x => x.Id)));
        }

        res.Data.Slots.AddRange(slots);
        await _roomRepository.UpdateAsync(res.Data);
            
        return Response.Ok(new { Message = "Successfully added slot to room."});
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
        var res = await _workspaceRepository.GetEntityByIdAsync(request.WorkspaceId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(request.WorkspaceId, "Workspace");

        List<EventCategory> eventCategories = [];
        if (request.EventCategoryIds != null)
        {
            foreach (var eventCategoryId in request.EventCategoryIds) // TODO refactor
            {
                var res2 = await _eventCategoryRepository.GetEntityByIdAsync(eventCategoryId);
                if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
                if (res2.Data == null) return Response.NotFound(eventCategoryId, "Event category");
                
                eventCategories.Add(res2.Data);
            }
        }

        var mainSlot = new Slot(res.Data, request.StartDate, request.EndDate, request.StartTime, request.EndTime, eventCategories, request.RepetitionInterval, request.RepetitionNumber,
            null, request.RepetitionEndDate)
        {
            Workspace = res.Data
        };

        List<Slot> slots = [mainSlot];
        var repetitionStartDate = Repetition.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
        var repetitionEndDate = Repetition.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);

        while (repetitionStartDate < request.RepetitionEndDate)
        {
            var slot = new Slot(res.Data, repetitionStartDate, repetitionEndDate, request.StartTime, request.EndTime, eventCategories, null, null, mainSlot, null)
            {
                Workspace = res.Data
            };
            slots.Add(slot);

            repetitionStartDate = Repetition.IncrementDateOnly(request.StartDate, request.RepetitionInterval, request.RepetitionNumber);
            repetitionEndDate = Repetition.IncrementDateOnly(request.EndDate, request.RepetitionInterval, request.RepetitionNumber);
        }
        await _slotRepository.CreateMultipleAsync(slots);

        return Response.CreatedAt(slots.Select(x => _mapper.Map<SlotDto>(x)).ToList());  
    }
}
