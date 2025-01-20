using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

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

    public async Task<ParticipantCategoryDto> CreateAsync(CreateParticipantCategoryRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId) ?? throw new NotFoundException("Workspace", request.WorkspaceId);
        var participantCategory = new ParticipantCategory(workspace, request.Name, request.Description, request.Color, request.Icon ) { Workspace = workspace };

        await _participantCategoryRepository.CreateAsync(participantCategory);
        
        return _mapper.Map<ParticipantCategoryDto>(participantCategory); 
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _participantCategoryRepository.DeleteAsync(id);
    }

    public async Task<ParticipantCategoryDto?> GetByIdAsync(string id)
    {
        var participantCategory = await _participantCategoryRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("PariticipantCategory", id);

        return _mapper.Map<ParticipantCategoryDto>(participantCategory);
    }

    public async Task<ParticipantCategoryDto> UpdateAsync(string id, UpdateParticipantCategoryRequest req)
    {
        var participantCategory = await _participantCategoryRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("ParticipantCategory", id);

        participantCategory.Name = req.Name ?? participantCategory.Name;
        participantCategory.Icon = req.Icon ?? participantCategory.Icon;
        participantCategory.Description = req.Description ?? participantCategory.Description;
        participantCategory.Color = req.Color ?? participantCategory.Color;

        await _participantCategoryRepository.UpdateAsync(participantCategory);

        return _mapper.Map<ParticipantCategoryDto>(participantCategory);
    }

    public async Task<List<ParticipantCategoryDto>> GetByWorkspaceIdAsync(string id) {
        var res = await _participantCategoryRepository.GetByWorkpaceIdAsync(id);
        return _mapper.Map<List<ParticipantCategoryDto>>(res);
    }
}
