using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;
// TODO voir si mettre les areas dans le LocationDto
public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public LocationService(ILocationRepository locationRepository, IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        _locationRepository = locationRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult<LocationDto>> GetByIdAsync(string id)
    {
        var location = await _locationRepository.GetByIdAsync<LocationDto>(id);
        if (location == null) return Response.NotFound(id, "Location");

        return Response.Ok(location);
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _locationRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        
        return Response.NoContent();
    }

    public async Task<ActionResult<LocationDto>> CreateAsync(string workspaceId, string name, string? description, string? address, string? city, string? country)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(workspaceId);
        if(!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(workspaceId, "Workspace");

        var location = new Location(res.Data, name, description, address, city, country) { Workspace = res.Data };

        var res2 = await _locationRepository.CreateAsync(location);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.CreatedAt(_mapper.Map<LocationDto>(location));
    }

    public async Task<ActionResult<LocationDto>> UpdateAsync(string id, string? newName, string? newDescription, string? newAddress, string? newCity, string? newCountry)
    {
        if (newName == null && newDescription == null && newAddress == null && newCity == null && newCountry == null) 
            return Response.BadRequest("At least one field is required.", null);
            
        var res = await _locationRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null ) return Response.NotFound(id, "Location");

        res.Data.Name = newName ?? res.Data.Name;
        res.Data.Description = newDescription ?? res.Data.Description;
        res.Data.Address = newAddress ?? res.Data.Address;
        res.Data.City = newCity ?? res.Data.Name;
        res.Data.Country = newCountry ?? res.Data.Country;

        var res2 = await _locationRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.Ok(_mapper.Map<LocationDto>(res.Data));
    }
}
