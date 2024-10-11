using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventCategoryService : IEventCategoryService
{
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    public EventCategoryService(IEventCategoryRepository eventCategoryRepository, IWorkspaceRepository workspaceRepository)
    {
        _eventCategoryRepository = eventCategoryRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<ActionResult<EventCategoryDto>> CreateAsync(string workspaceId, string name, string icon, string color)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        var eventCategory = new EventCategory(workspace, name, icon, color)
        {
            Workspace = workspace
        };

        await _eventCategoryRepository.CreateAsync(eventCategory);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "EventCategory", 
            routeValues: new { id = eventCategory.Id }, 
            value: new EventCategoryDto(eventCategory)
        );  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null) return new NotFoundObjectResult("Event category not found.");

        await _eventCategoryRepository.DeleteAsync(eventCategory);

        return new OkObjectResult("Successfully deleted event category.");
    }

    public async Task<ActionResult<EventCategoryDto>> GetByIdAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null) return new NotFoundObjectResult("Event category not found.");

        return new OkObjectResult(new EventCategoryDto(eventCategory));
    }

    public async Task<ActionResult<EventCategoryDto>> UpdateAsync(string id, string? newName, string? newIcon)
    {
        if (newName == null && newIcon == null) return new BadRequestObjectResult("At least one field is required.");

        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null ) return new NotFoundObjectResult("Event category not found.");

        eventCategory.Name = newName ?? eventCategory.Name;
        eventCategory.Icon = newIcon ?? eventCategory.Icon;

        await _eventCategoryRepository.UpdateAsync(eventCategory);
        return new OkObjectResult(new EventCategoryDto(eventCategory));
    }
}
