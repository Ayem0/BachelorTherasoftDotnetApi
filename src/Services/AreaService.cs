using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

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

    public async Task<Response<AreaDto?>> CreateAsync(string locationId, string name, string? description)
    {
        var location = await _locationRepository.GetByIdAsync(locationId);
        if (location == null) return new Response<AreaDto?>(success: false, errors: ["Location not found."]);

        var area = new Area(location, name, description)
        {
            Location = location,
        };
        await _areaRepository.CreateAsync(area);

        return new Response<AreaDto?>(success: true, content: new AreaDto(area));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null) return new Response<string>(success: false, errors: ["Area not found."]);

        await _areaRepository.DeleteAsync(area);
        return new Response<string>(success: true, content: "Area successfully deleted.");
    }

    public async Task<Response<AreaDto?>> GetByIdAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null) return new Response<AreaDto?>(success: false, errors: ["Area not found."]);

        return new Response<AreaDto?>(success: true, content: new AreaDto(area));
    }

    public async Task<Response<AreaDto?>> UpdateAsync(string id, string? newName, string? newDescription)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null || (newName == null && newDescription == null)) return new Response<AreaDto?>(success: false, errors: ["Area not found."]);

        area.Name = newName ?? area.Name;
        area.Description = newDescription ?? area.Description;

        await _areaRepository.UpdateAsync(area);
        return new Response<AreaDto?>(success: true, content: new AreaDto(area));
    }
}
