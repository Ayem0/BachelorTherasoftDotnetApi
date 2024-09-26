using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Repositories;

namespace BachelorTherasoftDotnetApi.src.Services;

public class AreaService : IAreaService
{
    private readonly AreaRepository _areaRepository;
    private readonly LocationRepository _locationRepository;
    public AreaService(AreaRepository areaRepository, LocationRepository locationRepository)
    {
        _areaRepository = areaRepository;
        _locationRepository = locationRepository;
    }

    public async Task<AreaDto?> CreateAsync(string name, string locationId)
    {
        var location = await _locationRepository.GetByIdAsync(locationId);
        if (location == null) return null;

        var area = new Area {
            Name = name,
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

    public async Task<bool> UpdateAsync(string id, string newName)
    {
        var area = await _areaRepository.GetByIdAsync(id);
        if (area == null) return false;

        area.Name = newName;

        await _areaRepository.UpdateAsync(area);

        return true;
    }
}
