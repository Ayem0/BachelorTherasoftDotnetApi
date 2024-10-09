using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

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

    public async Task<Response<ParticipantCategoryDto?>> CreateAsync(string workspaceId, string name, string icon)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return new Response<ParticipantCategoryDto?>(success: false, errors: ["Workspace not found."]);

        var participantCategory = new ParticipantCategory(workspace, name, icon) { Workspace = workspace };
        await _participantCategoryRepository.CreateAsync(participantCategory);

        return new Response<ParticipantCategoryDto?>(success: true, content: new ParticipantCategoryDto(participantCategory));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if (participantCategory == null) return new Response<string>(success: false, errors: ["Participant category not found."]);

        await _participantCategoryRepository.DeleteAsync(participantCategory);
        
        return new Response<string>(success: true, content: "Participant category successfully deleted.");
    }

    public async Task<Response<ParticipantCategoryDto?>> GetByIdAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if (participantCategory == null) return new Response<ParticipantCategoryDto?>(success: false, errors: ["Participant category not found."]);

        return new Response<ParticipantCategoryDto?>(success: true, content: new ParticipantCategoryDto(participantCategory));
    }

    public async Task<Response<ParticipantCategoryDto?>> UpdateAsync(string id, string? newName, string? newIcon)
    {
        if (newName == null && newIcon == null) return new Response<ParticipantCategoryDto?>(success: false, errors: ["At least one field is required."]);
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if (participantCategory == null ) return new Response<ParticipantCategoryDto?>(success: false, errors: ["Participant category not found."]);

        participantCategory.Name = newName ?? participantCategory.Name;
        participantCategory.Icon = newIcon ?? participantCategory.Icon;

        await _participantCategoryRepository.UpdateAsync(participantCategory);
        
        return new Response<ParticipantCategoryDto?>(success: true, content: new ParticipantCategoryDto(participantCategory));
    }
}
