using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public ParticipantService(IParticipantRepository participantRepository, IWorkspaceRepository workspaceRepository, IParticipantCategoryRepository participantCategoryRepository, IMapper mapper)
    {
        _participantRepository = participantRepository;
        _workspaceRepository = workspaceRepository;
        _participantCategoryRepository = participantCategoryRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult<ParticipantDto>> CreateAsync(CreateParticipantRequest request)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(request.WorkspaceId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(request.WorkspaceId, "Workspace");

        var res2 = await _participantCategoryRepository.GetEntityByIdAsync(request.ParticipantCategoryId);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        if (res2.Data == null) return Response.NotFound(request.ParticipantCategoryId, "Participant category");

        var participant = new Participant(res.Data, res2.Data, request.FirstName, request.LastName, request.Description, request.Email, request.PhoneNumber, request.Address, request.City, request.Country, request.DateOfBirth)
        {
            ParticipantCategory = res2.Data,
            Workspace = res.Data
        };

        var res3 = await _participantRepository.CreateAsync(participant);
        if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);

        return Response.CreatedAt(_mapper.Map<ParticipantDto>(participant));
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _participantRepository.DeleteAsync(id);
        if (res.Success) return Response.NoContent();

        return Response.BadRequest(res.Message, res.Details);
    }

    public async Task<ActionResult<ParticipantDto>> GetByIdAsync(string id)
    {
        var participant = await _participantRepository.GetByIdAsync<ParticipantDto>(id);
        if (participant == null) return Response.NotFound(id, "Participant");

        return Response.Ok(participant);
    }

    public async Task<ActionResult<ParticipantDto>> UpdateAsync(string id, UpdateParticipantRequest request)
    {
        if (request.NewParticipantCategoryId == null && request.NewFirstName == null && request.NewLastName == null && request.NewEmail == null &&
            request.NewDescription == null && request.NewAddress == null && request.NewCity == null && request.NewCountry == null && request.NewDateOfBirth == null)
            return new BadRequestObjectResult("At least one field is required.");

        var res = await _participantRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, "Participant");

        if (request.NewParticipantCategoryId != null)
        {
            var res2 = await _participantCategoryRepository.GetEntityByIdAsync(request.NewParticipantCategoryId);
            if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
            if (res2.Data == null) return Response.NotFound(request.NewParticipantCategoryId, "Participant category");

            res.Data.ParticipantCategory = res2.Data;
            res.Data.ParticipantCategoryId = res2.Data.Id;
        }

        res.Data.FirstName = request.NewFirstName ?? res.Data.FirstName;
        res.Data.LastName = request.NewLastName ?? res.Data.LastName;
        res.Data.Email = request.NewEmail ?? res.Data.Email;
        res.Data.Description = request.NewDescription ?? res.Data.Description;
        res.Data.Address = request.NewAddress ?? res.Data.Address;
        res.Data.City = request.NewCity ?? res.Data.City;
        res.Data.Country = request.NewCountry ?? res.Data.Country;
        res.Data.DateOfBirth = request.NewDateOfBirth ?? res.Data.DateOfBirth;

        var res3 = await _participantRepository.UpdateAsync(res.Data);
        if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);

        return Response.Ok(_mapper.Map<ParticipantDto>(res.Data));
    }
}
