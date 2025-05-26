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
// TODO voir si mettre les areas dans le LocationDto
public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;
    public LocationService(
        ILocationRepository locationRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        ISocketService socket
    )
    {
        _locationRepository = locationRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _socket = socket;
    }

    public async Task<LocationDto> CreateAsync(CreateLocationRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var location = new Location(workspace, req.Name, req.Description, req.Address, req.City, req.Country) { Workspace = workspace };
        var created = await _locationRepository.CreateAsync(location);
        var dto = _mapper.Map<LocationDto>(created);
        await _socket.NotififyGroup(req.WorkspaceId, "LocationCreated", dto);
        return dto;
    }

    public async Task<LocationDto> UpdateAsync(string id, UpdateLocationRequest req)
    {
        var location = await _locationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Location", id);

        location.Name = req.Name ?? location.Name;
        location.Description = req.Description ?? location.Description;

        var updated = await _locationRepository.UpdateAsync(location);
        var dto = _mapper.Map<LocationDto>(updated);
        await _socket.NotififyGroup(location.WorkspaceId, "LocationUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var location = await _locationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Location", id);
        var success = await _locationRepository.DeleteAsync(location);
        if (success)
        {
            await _socket.NotififyGroup(location.WorkspaceId, "LocationDeleted", id);
        }
        return success;
    }

    public async Task<LocationDto?> GetByIdAsync(string id)
    => _mapper.Map<LocationDto>(await _locationRepository.GetByIdAsync(id));

    public async Task<List<LocationDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<LocationDto>>(await _locationRepository.GetByWorkspaceIdAsync(workspaceId));

}
