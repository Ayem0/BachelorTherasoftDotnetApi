using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;
// TODO voir si mettre les areas dans le LocationDto
public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public LocationService(ILocationRepository locationRepository, IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        _locationRepository = locationRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    public async Task<LocationDto?> GetByIdAsync(string id)
    {
        var location = await _locationRepository.GetDetailsByIdAsync(id) ?? throw new NotFoundException("Location", id);

        return location != null ? _mapper.Map<LocationDto>(location) : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _locationRepository.DeleteAsync(id);
    }

    public async Task<LocationDto> CreateAsync(CreateLocationRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);

        var location = new Location(workspace, req.Name, req.Description, req.Address, req.City, req.Country) { Workspace = workspace };
        await _locationRepository.CreateAsync(location);

        return _mapper.Map<LocationDto>(location);
    }

    public async Task<LocationDto> UpdateAsync(string id, UpdateLocationRequest req)
    {
        var location = await _locationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Location", id);

        location.Name = req.Name ?? location.Name;
        location.Description = req.Description ?? location.Description;
        location.Address = req.Address ?? location.Address;
        location.City = req.City ?? location.City;
        location.Country = req.Country ?? location.Country;

        await _locationRepository.UpdateAsync(location);

        return _mapper.Map<LocationDto>(location);
    }

    public async Task<List<LocationDto>> GetByWorkspaceIdAsync(string workspaceId)
    {
        var locations = await _locationRepository.GetByWorkspaceIdAsync(workspaceId);

        return _mapper.Map<List<LocationDto>>(locations);
    }
}
