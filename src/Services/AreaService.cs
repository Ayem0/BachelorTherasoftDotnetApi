using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<AreaDto>> CreateAsync(string locationId, string name, string? description)
    {
        var res = await _locationRepository.GetEntityByIdAsync(locationId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(locationId, "Location not found.");

        var area = new Area(res.Data, name, description)
        {
            Location = res.Data,
        };
        
        var res2 = await _areaRepository.CreateAsync(area);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        
        return Response.CreatedAt(_mapper.Map<AreaDto>(area));
    }


    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _areaRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);

        return Response.Ok("Area successfully deleted.");
    }

    public async Task<ActionResult<AreaDto>> GetByIdAsync(string id)
    {
        var area = await _areaRepository.GetByIdAsync<AreaDto>(id);
        if (area == null) return Response.NotFound(id, "Area not found.");

        return Response.Ok(area);
    }

    public async Task<ActionResult<AreaDto>> UpdateAsync(string id, string? newName, string? newDescription)
    {
        if (newName == null && newDescription == null) return Response.BadRequest("At least one field is required.", null);

        var res = await _areaRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null ) return Response.NotFound(id, "Area not found.");

        res.Data.Name = newName ?? res.Data.Name;
        res.Data.Description = newDescription ?? res.Data.Description;

        var res2 = await _areaRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        
        return Response.Ok(_mapper.Map<AreaDto>(res.Data));
    }
}
