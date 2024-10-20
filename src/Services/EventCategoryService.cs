using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

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

    public async Task<EventCategoryDto> CreateAsync(CreateEventCategoryRequest req)
    {
        var workspace = await _workspaceRepository.GetEntityByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);

        var eventCategory = new EventCategory(workspace, req.Name, req.Icon, req.Color){ Workspace = workspace };

        await _eventCategoryRepository.CreateAsync(eventCategory);

        return _mapper.Map<EventCategoryDto>(eventCategory);  
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _eventCategoryRepository.DeleteAsync(id);
    }

    public async Task<EventCategoryDto> GetByIdAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("EventCategory", id);

        return _mapper.Map<EventCategoryDto>(eventCategory);
    }

    public async Task<EventCategoryDto> UpdateAsync(string id, UpdateEventCategoryRequest req)
    {
        var eventCategory = await _eventCategoryRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("EventCategory", id);

        eventCategory.Name = req.NewName ?? eventCategory.Name;
        eventCategory.Icon = req.NewIcon ?? eventCategory.Icon;

        await _eventCategoryRepository.UpdateAsync(eventCategory);

        return _mapper.Map<EventCategoryDto>(eventCategory);
    }
}
