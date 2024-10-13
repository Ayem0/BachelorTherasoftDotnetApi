using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantCategoryService : IParticipantCategoryService
{
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public ParticipantCategoryService(IParticipantCategoryRepository participantCategoryRepository, IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        _participantCategoryRepository = participantCategoryRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult<ParticipantCategoryDto>> CreateAsync(string workspaceId, string name, string icon)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(workspaceId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(workspaceId, "Workspace not found.");

        var participantCategory = new ParticipantCategory(res.Data, name, icon) { Workspace = res.Data };

        var res2 = await _participantCategoryRepository.CreateAsync(participantCategory);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        
        return Response.Ok(_mapper.Map<ParticipantCategoryDto>(participantCategory)); 
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _participantCategoryRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);

        return Response.NoContent();
    }

    public async Task<ActionResult<ParticipantCategoryDto>> GetByIdAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync<ParticipantCategoryDto>(id);
        if (participantCategory == null) return Response.NotFound(id, "Participant category not found.");

        return Response.Ok(participantCategory);
    }

    public async Task<ActionResult<ParticipantCategoryDto>> UpdateAsync(string id, string? newName, string? newIcon)
    {
        if (newName == null && newIcon == null) return new BadRequestObjectResult("At least one field is required.");

        var res = await _participantCategoryRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null ) return Response.NotFound(id, "Participant category not found.");

        res.Data.Name = newName ?? res.Data.Name;
        res.Data.Icon = newIcon ?? res.Data.Icon;

        var res2 = await _participantCategoryRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.Ok(_mapper.Map<ParticipantCategoryDto>(res.Data));
    }
}
