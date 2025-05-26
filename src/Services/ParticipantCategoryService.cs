using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantCategoryService : IParticipantCategoryService
{
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;
    public ParticipantCategoryService(
        IParticipantCategoryRepository participantCategoryRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        ISocketService socket
        )
    {
        _participantCategoryRepository = participantCategoryRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _socket = socket;
    }

    public async Task<ParticipantCategoryDto> CreateAsync(CreateParticipantCategoryRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var pc = new ParticipantCategory(workspace, req.Name, req.Description, req.Color, req.Icon) { Workspace = workspace };
        var created = await _participantCategoryRepository.CreateAsync(pc);
        var dto = _mapper.Map<ParticipantCategoryDto>(created);
        await _socket.NotififyGroup(req.WorkspaceId, "ParticipantCategoryCreated", dto);
        return dto;
    }

    public async Task<ParticipantCategoryDto> UpdateAsync(string id, UpdateParticipantCategoryRequest req)
    {
        var pc = await _participantCategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("ParticipantCategory", id);

        pc.Name = req.Name ?? pc.Name;
        pc.Description = req.Description ?? pc.Description;

        var updated = await _participantCategoryRepository.UpdateAsync(pc);
        var dto = _mapper.Map<ParticipantCategoryDto>(updated);
        await _socket.NotififyGroup(updated.WorkspaceId, "ParticipantCategoryUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var pc = await _participantCategoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("ParticipantCategory", id);
        var success = await _participantCategoryRepository.DeleteAsync(pc);
        if (success)
        {
            await _socket.NotififyGroup(pc.WorkspaceId, "ParticipantCategoryDeleted", id);
        }
        return success;
    }

    public async Task<ParticipantCategoryDto?> GetByIdAsync(string id)
    => _mapper.Map<ParticipantCategoryDto?>(await _participantCategoryRepository.GetByIdAsync(id));

    public async Task<List<ParticipantCategoryDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<ParticipantCategoryDto>>(await _participantCategoryRepository.GetByWorkpaceIdAsync(workspaceId));
}
