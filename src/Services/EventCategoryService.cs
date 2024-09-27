using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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

    public async Task<EventCategoryDto?> CreateAsync(string workspaceId, string name, string icon, string color)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var eventCategory = new EventCategory(workspace, name, icon, color) {
            Workspace = workspace
        };

        await _eventCategoryRepository.CreateAsync(eventCategory);
        return new EventCategoryDto(eventCategory);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null) return false;

        await _eventCategoryRepository.DeleteAsync(eventCategory);
        return true;
    }

    public async Task<EventCategoryDto?> GetByIdAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null) return null;

        return new EventCategoryDto(eventCategory);
    }

    public async Task<EventCategoryDto?> UpdateAsync(string id, string? newName, string? newIcon)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id);
        if (eventCategory == null || (newName == null && newIcon == null)) return null;

        eventCategory.Name = newName ?? eventCategory.Name;
        eventCategory.Icon = newIcon ?? eventCategory.Icon;

        await _eventCategoryRepository.UpdateAsync(eventCategory);
        return new EventCategoryDto(eventCategory);
    }
}
