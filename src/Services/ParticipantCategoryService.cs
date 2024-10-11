using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantCategoryService : IParticipantCategoryService
{
    private readonly IParticipantCategoryRepository _participantCategoryRepository;

    private readonly IWorkspaceRepository _workspaceRepository;
    public ParticipantCategoryService(IParticipantCategoryRepository participantCategoryRepository, IWorkspaceRepository workspaceRepository)
    {
        _participantCategoryRepository = participantCategoryRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<ActionResult<ParticipantCategoryDto>> CreateAsync(string workspaceId, string name, string icon)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        var participantCategory = new ParticipantCategory(workspace, name, icon) { Workspace = workspace };
        await _participantCategoryRepository.CreateAsync(participantCategory);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "ParticipantCategory", 
            routeValues: new { id = participantCategory.Id }, 
            value: new ParticipantCategoryDto(participantCategory)
        );  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if (participantCategory == null) return new NotFoundObjectResult("Participant category not found.");

        await _participantCategoryRepository.DeleteAsync(participantCategory);
        
        return new OkObjectResult("Participant category successfully deleted.");
    }

    public async Task<ActionResult<ParticipantCategoryDto>> GetByIdAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if (participantCategory == null) return new NotFoundObjectResult("Participant category not found.");

        return new OkObjectResult(new ParticipantCategoryDto(participantCategory));
    }

    public async Task<ActionResult<ParticipantCategoryDto>> UpdateAsync(string id, string? newName, string? newIcon)
    {
        if (newName == null && newIcon == null) return new BadRequestObjectResult("At least one field is required.");
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if (participantCategory == null ) return new NotFoundObjectResult("Participant category not found.");

        participantCategory.Name = newName ?? participantCategory.Name;
        participantCategory.Icon = newIcon ?? participantCategory.Icon;

        await _participantCategoryRepository.UpdateAsync(participantCategory);
        
        return new OkObjectResult(new ParticipantCategoryDto(participantCategory));
    }
}
