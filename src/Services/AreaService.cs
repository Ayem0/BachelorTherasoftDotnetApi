using System.Text.Json;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace BachelorTherasoftDotnetApi.src.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly ILogger<AreaService> _logger;
    public AreaService(
        IAreaRepository areaRepository,
        ILocationRepository locationRepository,
        IMapper mapper,
        IDistributedCache cache,
        ILogger<AreaService> logger
    )
    {
        _areaRepository = areaRepository;
        _locationRepository = locationRepository;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<AreaDto> CreateAsync(CreateAreaRequest request)
    {
        var location = await _locationRepository.GetByIdJoinWorkspaceAsync(request.LocationId) ?? throw new NotFoundException("Location", request.LocationId);

        var area = new Area(location.Workspace, location, request.Name, request.Description) { Location = location, Workspace = location.Workspace };

        await _areaRepository.CreateAsync(area);

        return _mapper.Map<AreaDto>(area);
    }


    public async Task<bool> DeleteAsync(string id)
    {
        return await _areaRepository.DeleteAsync(id);
    }

    public async Task<AreaDto> GetByIdAsync(string id)
    {
        var cacheKey = $"area:{id}";
        var cacheValue = await _cache.GetStringAsync(cacheKey);
        if (cacheValue != null)
        {
            _logger.LogInformation("Area loaded from cache");
            var fromCache = JsonSerializer.Deserialize<AreaDto>(cacheValue)!;
            return fromCache;
        }
        _logger.LogInformation("Area not loaded from cache");
        var area = await _areaRepository.GetByIdAsync(id) ?? throw new NotFoundException("Area", id);
        var areaDto = _mapper.Map<AreaDto>(area);
        var serialized = JsonSerializer.Serialize(areaDto);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        };
        await _cache.SetStringAsync(cacheKey, serialized, options);
        return areaDto;
    }

    public async Task<AreaDto> UpdateAsync(string id, UpdateAreaRequest req)
    {
        var area = await _areaRepository.GetByIdAsync(id) ?? throw new NotFoundException("Area", id);

        area.Name = req.Name ?? area.Name;
        area.Description = req.Description ?? area.Description;

        await _areaRepository.UpdateAsync(area);

        return _mapper.Map<AreaDto>(area);
    }

    public async Task<List<AreaDto>> GetAreasByLocationIdAsync(string locationId)
    {
        var areas = await _areaRepository.GetAreasByLocationIdAsync(locationId);

        return _mapper.Map<List<AreaDto>>(areas);
    }
}
