using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly ILocationRepository _locationRepository;
    public AreaService(IAreaRepository areaRepository, ILocationRepository locationRepository)
    {
        _areaRepository = areaRepository;
        _locationRepository = locationRepository;
    }

    public async Task<ActionResult<AreaDto>> CreateAsync(string locationId, string name, string? description)
    {
        var location = await _locationRepository.GetByIdAsync(locationId);
        if (location == null) return new NotFoundObjectResult("Location not found.");

        var area = new Area(location, name, description)
        {
            Location = location,
        };
        await _areaRepository.CreateAsync(area);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Area", 
            routeValues: new { id = area.Id }, 
            value: new AreaDto(area)
        );  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null) return new NotFoundObjectResult("Area not found.");

        await _areaRepository.DeleteAsync(area);
        return new OkObjectResult("Area successfully deleted.");
    }

    public async Task<ActionResult<AreaDto>> GetByIdAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null) return new NotFoundObjectResult("Area not found.");

        return new OkObjectResult(new AreaDto(area));
    }

    public async Task<ActionResult<AreaDto>> UpdateAsync(string id, string? newName, string? newDescription)
    {
        if (newName == null && newDescription == null) return new BadRequestObjectResult("At least one field is required.");
        
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null ) return new NotFoundObjectResult("Area not found.");;

        area.Name = newName ?? area.Name;
        area.Description = newDescription ?? area.Description;

        await _areaRepository.UpdateAsync(area);
        return new OkObjectResult(new AreaDto(area));
    }
}
