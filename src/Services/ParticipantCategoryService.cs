using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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

    public async Task<ParticipantCategoryDto?> CreateAsync(string workspaceId, string name, string icon)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if(workspace == null) return null;

        var participantCategory = new ParticipantCategory {
            Icon = icon,
            Name = name,
            Workspace = workspace,
            WorkspaceId = workspace.Id
        };

        await _participantCategoryRepository.CreateAsync(participantCategory);

        var participantCategoryDto = new ParticipantCategoryDto {
            Id = participantCategory.Id,
            Name = participantCategory.Name,
            Icon = participantCategory.Icon
        };
        return participantCategoryDto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if(participantCategory == null) return false;
        await _participantCategoryRepository.DeleteAsync(participantCategory);
        return true;
    }

    public async Task<ParticipantCategoryDto?> GetByIdAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if(participantCategory == null) return null;
        
        var participantCategoryDto = new ParticipantCategoryDto {
            Id = participantCategory.Id,
            Name = participantCategory.Name,
            Icon = participantCategory.Icon
        };

        return participantCategoryDto;
    }

    public async Task<bool> UpdateAsync(string id, string? newName, string? newIcon)
    {
        var participantCategory = await _participantCategoryRepository.GetByIdAsync(id);
        if(participantCategory == null || (newName == null && newIcon == null)) return false;

        participantCategory.Name = newName ?? participantCategory.Name;
        participantCategory.Icon = newIcon ?? participantCategory.Icon;

        await _participantCategoryRepository.UpdateAsync(participantCategory);

        return true;
    }
}
