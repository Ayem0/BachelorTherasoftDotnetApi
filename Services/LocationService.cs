using System;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Repositories;

namespace BachelorTherasoftDotnetApi.Services;

public class LocationService : ILocationService
{
    private readonly LocationRepository _locationRepository;

    public LocationService(LocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }
    
    public async Task CreateLocationAsync(Location location)
    {
        await _locationRepository.CreateAsync(location);
    }

    public async Task DeleteLocationAsync(string id)
    {
        await _locationRepository.DeleteAsync(id);
    }

    public async Task<Location?> GetLocationByIdAsync(string id)
    {
        return await _locationRepository.GetByIdAsync(id);
    }

    public async Task UpdateLocationAsync(Location location)
    {
        await _locationRepository.UpdateAsync(location);
    }
}
