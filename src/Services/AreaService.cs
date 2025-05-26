using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;

    public AreaService(
        IAreaRepository areaRepository,
        ILocationRepository locationRepository,
        IMapper mapper,
        ISocketService socket,
        IWorkspaceRepository workspaceRepository
    )
    {
        _areaRepository = areaRepository;
        _locationRepository = locationRepository;
        _mapper = mapper;
        _socket = socket;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<AreaDto> CreateAsync(CreateAreaRequest req)
    {
        var location = await _locationRepository.GetByIdAsync(req.LocationId) ?? throw new NotFoundException("Location", req.LocationId);
        var workspace = await _workspaceRepository.GetByIdAsync(location.WorkspaceId) ?? throw new NotFoundException("Workspace", location.WorkspaceId);
        var area = new Area(workspace, location, req.Name, req.Description) { Location = location, Workspace = workspace };
        var created = await _areaRepository.CreateAsync(area);
        var dto = _mapper.Map<AreaDto>(created);
        await _socket.NotififyGroup(created.WorkspaceId, "AreaCreated", dto);
        return dto;
    }

    public async Task<AreaDto> UpdateAsync(string id, UpdateAreaRequest req)
    {
        var area = await _areaRepository.GetByIdAsync(id) ?? throw new NotFoundException("Area", id);

        area.Name = req.Name ?? area.Name;
        area.Description = req.Description ?? area.Description;

        var updated = await _areaRepository.UpdateAsync(area);
        var dto = _mapper.Map<AreaDto>(updated);
        await _socket.NotififyGroup(updated.WorkspaceId, "AreaUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync(id) ?? throw new NotFoundException("Area", id);
        var success = await _areaRepository.DeleteAsync(area);
        if (success)
        {
            await _socket.NotififyGroup(area.WorkspaceId, "AreaDeleted", id);
        }
        return success;
    }

    public async Task<AreaDto?> GetByIdAsync(string id)
    => _mapper.Map<AreaDto?>(await _areaRepository.GetByIdAsync(id));

    public async Task<List<AreaDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<AreaDto>>(await _areaRepository.GetAreasByWorkspaceIdAsync(workspaceId));

    public async Task<List<AreaDto>> GetByLocationIdAsync(string locationId)
    => _mapper.Map<List<AreaDto>>(await _areaRepository.GetAreasByLocationIdAsync(locationId));
}
