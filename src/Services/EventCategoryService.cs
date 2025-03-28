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
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);

        var eventCategory = new EventCategory(workspace, req.Name, req.Icon, req.Color, req.Description) { Workspace = workspace };

        await _eventCategoryRepository.CreateAsync(eventCategory);

        return _mapper.Map<EventCategoryDto>(eventCategory);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _eventCategoryRepository.DeleteAsync(id);
    }

    public async Task<EventCategoryDto> GetByIdAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("EventCategory", id);

        return _mapper.Map<EventCategoryDto>(eventCategory);
    }

    public async Task<EventCategoryDto> UpdateAsync(string id, UpdateEventCategoryRequest req)
    {
        var eventCategory = await _eventCategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("EventCategory", id);

        eventCategory.Name = req.Name ?? eventCategory.Name;
        eventCategory.Icon = req.Icon ?? eventCategory.Icon;
        eventCategory.Color = req.Color ?? eventCategory.Color;
        eventCategory.Description = req.Description ?? eventCategory.Description;

        await _eventCategoryRepository.UpdateAsync(eventCategory);

        return _mapper.Map<EventCategoryDto>(eventCategory);
    }

    public async Task<List<EventCategoryDto>> GetByWorkspaceIdAsync(string id)
    {
        var eventCategory = await _eventCategoryRepository.GetByWorkpaceIdAsync(id);

        return _mapper.Map<List<EventCategoryDto>>(eventCategory);
    }
}
