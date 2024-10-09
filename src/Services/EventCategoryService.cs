using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

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

    public async Task<Response<EventCategoryDto?>> CreateAsync(string workspaceId, string name, string icon, string color)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return new Response<EventCategoryDto?>(success: false, errors: ["Workspace not found."]);

        var eventCategory = new EventCategory(workspace, name, icon, color)
        {
            Workspace = workspace
        };

        await _eventCategoryRepository.CreateAsync(eventCategory);

        return new Response<EventCategoryDto?>(success: true, content: new EventCategoryDto(eventCategory));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null) return new Response<string>(success: false, errors: ["Event category not found."]);

        await _eventCategoryRepository.DeleteAsync(eventCategory);
        return new Response<string>(success: true, content: "Event category successfully deleted.");
    }

    public async Task<Response<EventCategoryDto?>> GetByIdAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null) return new Response<EventCategoryDto?>(success: false, errors: ["Event category not found."]);

        return new Response<EventCategoryDto?>(success: true, content: new EventCategoryDto(eventCategory));
    }

    public async Task<Response<EventCategoryDto?>> UpdateAsync(string id, string? newName, string? newIcon)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null || (newName == null && newIcon == null)) 
            return new Response<EventCategoryDto?>(success: false, errors: ["Event category not found."]);

        eventCategory.Name = newName ?? eventCategory.Name;
        eventCategory.Icon = newIcon ?? eventCategory.Icon;

        await _eventCategoryRepository.UpdateAsync(eventCategory);
        return new Response<EventCategoryDto?>(success: true, content: new EventCategoryDto(eventCategory));
    }
}
