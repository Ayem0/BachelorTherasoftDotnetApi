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

    public async Task<AreaDto?> CreateAsync(string locationId, string name, string? description)
    {
        var location = await _locationRepository.GetByIdAsync(locationId);
        if (location == null) return null;

        var area = new Area(location, name, description)
        {
            Location = location,
        };
        await _areaRepository.CreateAsync(area);

        return new AreaDto(area);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null) return false;

        await _areaRepository.DeleteAsync(area);
        return true;
    }

    public async Task<AreaDto?> GetByIdAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null) return null;

        return new AreaDto(area);
    }

    public async Task<AreaDto?> UpdateAsync(string id, string? newName, string? newDescription)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null || (newName == null && newDescription == null)) return null;

        area.Name = newName ?? area.Name;
        area.Description = newDescription ?? area.Description;

        await _areaRepository.UpdateAsync(area);
        return new AreaDto(area);
    }
}
