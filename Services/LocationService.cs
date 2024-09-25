using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Repositories;

namespace BachelorTherasoftDotnetApi.Services;

public class LocationService : ILocationService
{
    private readonly LocationRepository _locationRepository;
    private readonly WorkspaceRepository _workspaceRepository;
    public LocationService(LocationRepository locationRepository, WorkspaceRepository workspaceRepository)
    {
        _locationRepository = locationRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<LocationDto?> GetByIdAsync(string id) 
    {
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null) return null;

        var locationDto = new LocationDto {
            Id = location.Id,
            Name = location.Name
            // Ajouter les areas 
        };
        return locationDto;
    }

    public async Task<bool> DeleteAsync(string id) 
    {
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null) return false;
        await _locationRepository.DeleteAsync(location);
        return true;
    }

    public async Task<LocationDto?> CreateAsync(string name, string workspaceId) 
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var location = new Location {
            Name = name,
            WorkspaceId = workspaceId,
            Workspace = workspace
        };

        await _locationRepository.CreateAsync(location);

        var locationDto = new LocationDto {
            Id = location.Id,
            Name = location.Name
        };

        return locationDto;
    }

    public async Task<bool> UpdateAsync(string id, string newName) 
    {
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null) return false;

        location.Name = newName;

        await _locationRepository.UpdateAsync(location);
        
        return true;
    }
}
