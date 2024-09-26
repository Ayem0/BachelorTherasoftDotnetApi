using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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

        var area = new Area {
            Name = name,
            Description = description,
            Location = location,
            LocationId = location.Id
        };

        await _areaRepository.CreateAsync(area);

        var areaDto = new AreaDto {
            Id = area.Id,
            Name = area.Name,
        };

        return areaDto;
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

        var areaDto = new AreaDto {
            Id = area.Id,
            Name = area.Name,
        };

        return areaDto;
    }

    public async Task<bool> UpdateAsync(string id, string? newName, string? newDescription)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null || (newName == null && newDescription == null)) return false;

        area.Name = newName ?? area.Name;
        area.Description = newDescription ?? area.Description;

        await _areaRepository.UpdateAsync(area);

        return true;
    }
}
