using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;
// TODO voir si mettre les areas dans le LocationDto
public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    public LocationService(ILocationRepository locationRepository, IWorkspaceRepository workspaceRepository)
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
            Name = location.Name,
            Description = location.Description,
            Address = location.Address,
            City = location.City,
            Country = location.Country
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

    public async Task<LocationDto?> CreateAsync(string workspaceId, string name, string? description, string? address, string? city, string? country) 
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var location = new Location {
            Name = name,
            WorkspaceId = workspaceId,
            Workspace = workspace,
            Description = description,
            Address = address,
            City = city,
            Country = country
        };

        await _locationRepository.CreateAsync(location);

        var locationDto = new LocationDto {
            Id = location.Id,
            Name = location.Name,
            Description = location.Description,
            Address = location.Address,
            City = location.City,
            Country = location.Country
        };

        return locationDto;
    }

    public async Task<bool> UpdateAsync(string id, string? newName, string? newDescription, string? newAddress, string? newCity, string? newCountry) 
    {
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null || (newName == null && newDescription == null && newAddress == null && newCity == null && newCountry == null)) return false;

        location.Name = newName ?? location.Name;
        location.Description = newDescription ?? location.Description;
        location.Address = newAddress ?? location.Address;
        location.City = newCity ?? location.Name;
        location.Country = newCountry ?? location.Country;

        await _locationRepository.UpdateAsync(location);
        
        return true;
    }
}
