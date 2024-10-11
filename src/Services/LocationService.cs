using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<LocationDto>> GetByIdAsync(string id)
    {
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null) return new NotFoundObjectResult("Location not found.");

        return new OkObjectResult(new LocationDto(location));
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null) return new NotFoundObjectResult("Location not found.");

        await _locationRepository.DeleteAsync(location);
        return new OkObjectResult("Location successfully deleted.");
    }

    public async Task<ActionResult<LocationDto>> CreateAsync(string workspaceId, string name, string? description, string? address, string? city, string? country)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return new NotFoundObjectResult("Location not found.");

        var location = new Location(workspace, name, description, address, city, country) { Workspace = workspace };
        await _locationRepository.CreateAsync(location);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Location", 
            routeValues: new { id = location.Id }, 
            value: new LocationDto(location)
        );  
    }

    public async Task<ActionResult<LocationDto>> UpdateAsync(string id, string? newName, string? newDescription, string? newAddress, string? newCity, string? newCountry)
    {
        if (newName == null && newDescription == null && newAddress == null && newCity == null && newCountry == null) 
            return new NotFoundObjectResult("At least one field is required.");
            
        var location = await _locationRepository.GetByIdAsync(id);
        if (location == null ) return new NotFoundObjectResult("Location not found.");

        location.Name = newName ?? location.Name;
        location.Description = newDescription ?? location.Description;
        location.Address = newAddress ?? location.Address;
        location.City = newCity ?? location.Name;
        location.Country = newCountry ?? location.Country;

        await _locationRepository.UpdateAsync(location);
        return new OkObjectResult(new LocationDto(location));
    }
}
