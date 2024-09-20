using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IAreaRepository
{
    Task<Area?> GetAreaAsync(string areaId);
    Task<List<Area>?> GetAreasAsync(string[] areasId);
    Task CreateAsync(string name, string locationId);
    Task DeleteAsync(string areaId);
    Task UpdateAsync(string areaId, string name);
}
