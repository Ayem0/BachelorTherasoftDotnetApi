using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventCategoryService : IEventCategoryService
{
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public EventCategoryService(IEventCategoryRepository eventCategoryRepository, IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        _eventCategoryRepository = eventCategoryRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult<EventCategoryDto>> CreateAsync(string workspaceId, string name, string icon, string color)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(workspaceId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(workspaceId, "Workspace");

        var eventCategory = new EventCategory(res.Data, name, icon, color)
        {
            Workspace = res.Data
        };

        var res2 = await _eventCategoryRepository.CreateAsync(eventCategory);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.CreatedAt(_mapper.Map<EventCategoryDto>(eventCategory));  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _eventCategoryRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        return Response.NoContent();
    }

    public async Task<ActionResult<EventCategoryDto>> GetByIdAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync<EventCategoryDto>(id);
        if (eventCategory == null) return Response.NotFound(id, "Event category");

        return Response.Ok(eventCategory);
    }

    public async Task<ActionResult<EventCategoryDto>> UpdateAsync(string id, string? newName, string? newIcon)
    {
        if (newName == null && newIcon == null) return Response.BadRequest("At least one field is required.", null);

        var res = await _eventCategoryRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null ) return Response.NotFound(id, "Event category");

        res.Data.Name = newName ?? res.Data.Name;
        res.Data.Icon = newIcon ?? res.Data.Icon;

        var res2 = await _eventCategoryRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.Ok(_mapper.Map<EventCategoryDto>(res.Data));
    }
}
