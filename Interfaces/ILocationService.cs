using System;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface ILocationService
{
    Task<Location?> GetLocationByIdAsync(string id);
    Task CreateLocationAsync(Location location);
    Task UpdateLocationAsync(Location location);
    Task DeleteLocationAsync(string id);
}
