using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    public AreaService(IAreaRepository areaRepository, ILocationRepository locationRepository, IMapper mapper)
    {
        _areaRepository = areaRepository;
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<AreaDto> CreateAsync(CreateAreaRequest request)
    {
        var location = await _locationRepository.GetByIdAsync(request.LocationId) ?? throw new NotFoundException("Location", request.LocationId);

        var area = new Area(location.Workspace, location, request.Name, request.Description){ Location = location, Workspace = location.Workspace };
        
        await _areaRepository.CreateAsync(area);
        
        return _mapper.Map<AreaDto>(area);
    }


    public async Task<bool> DeleteAsync(string id)
    {
        return await _areaRepository.DeleteAsync(id);
    }

    public async Task<AreaDto> GetByIdAsync(string id)
    {
        var area = await _areaRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("Area", id);

        return _mapper.Map<AreaDto>(area);
    }

    public async Task<AreaDto> UpdateAsync(string id, UpdateAreaRequest req)
    {
        var area = await _areaRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("Area", id);

        area.Name = req.NewName ?? area.Name;
        area.Description = req.NewDescription ?? area.Description;

        await _areaRepository.UpdateAsync(area);
        
        return _mapper.Map<AreaDto>(area);
    }

    public async Task<List<AreaDto>> GetAreasByLocationIdAsync(string locationId) {
        var areas = await _areaRepository.GetAreasByLocationIdAsync(locationId);
        
        return _mapper.Map<List<AreaDto>>(areas);
    }
}
