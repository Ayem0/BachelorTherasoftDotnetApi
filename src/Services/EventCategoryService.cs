using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventCategoryService : IEventCategoryService
{
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;

    public EventCategoryService(
        IEventCategoryRepository eventCategoryRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        ISocketService socket
    )
    {
        _eventCategoryRepository = eventCategoryRepository;
        _workspaceRepository = workspaceRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _socket = socket;
    }

    public async Task<EventCategoryDto> CreateAsync(CreateEventCategoryRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var eventCategory = new EventCategory(workspace, req.Name, req.Color, req.Description) { Workspace = workspace };
        var created = await _eventCategoryRepository.CreateAsync(eventCategory);
        var dto = _mapper.Map<EventCategoryDto>(created);
        await _socket.NotififyGroup(req.WorkspaceId, "EventCategoryCreated", dto);
        return dto;
    }

    public async Task<EventCategoryDto> UpdateAsync(string id, UpdateEventCategoryRequest req)
    {
        var ec = await _eventCategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("EventCategory", id);

        ec.Name = req.Name ?? ec.Name;
        ec.Color = req.Color ?? ec.Color;
        ec.Description = req.Description ?? ec.Description;

        var updated = await _eventCategoryRepository.UpdateAsync(ec);
        var dto = _mapper.Map<EventCategoryDto>(updated);
        await _socket.NotififyGroup(updated.WorkspaceId, "EventCategoryUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var ec = await _eventCategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("EventCategory", id);
        var success = await _eventCategoryRepository.DeleteAsync(ec);
        if (success)
        {
            await _socket.NotififyGroup(ec.WorkspaceId, "EventCategoryDeleted", id);
        }
        return success;
    }

    public async Task<EventCategoryDto?> GetByIdAsync(string id)
    => _mapper.Map<EventCategoryDto>(await _eventCategoryRepository.GetByIdAsync(id));

    public async Task<List<EventCategoryDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<EventCategoryDto>>(await _eventCategoryRepository.GetByWorkpaceIdAsync(workspaceId));
}
