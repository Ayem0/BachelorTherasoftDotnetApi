using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

// TODO ajouter le websocketservice qui n'existe pas encore
public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IEventRepository _eventRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    public InvitationService(IInvitationRepository invitationRepository, IWorkspaceRepository workspaceRepository, IEventRepository eventRepository, UserManager<User> userManager, IMapper mapper)
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<ActionResult> AcceptAsync(string id, User user)
    {
        var res = await _invitationRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, "Invitation");
        if (res.Data.ReceiverId != user.Id) return Response.Unauthorized();

        if (res.Data.InvitationType == InvitationType.Workspace && res.Data.Workspace != null)
        {
            var workspaceUser = new WorkspaceUser(user, res.Data.Workspace) { User = user, Workspace = res.Data.Workspace };

            res.Data.Workspace.Users.Add(workspaceUser); // TODO A REFACTOR LES 2 REQUETES DE DESSOUS

            var res2 = await _workspaceRepository.UpdateAsync(res.Data.Workspace);
            if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

            var res3 = await _invitationRepository.DeleteAsync(res.Data.Id);
            if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);

            return Response.Ok("Successfully accepted invitation.");
        }
        else if (res.Data.InvitationType == InvitationType.Event && res.Data.Event != null)
        {
            var eventUser = res.Data.Event.Users.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (eventUser == null) return Response.BadRequest("User is not a member of the event.", null);

            eventUser.Status = Status.Accepted;
            var res2 = await _eventRepository.UpdateAsync(res.Data.Event);
            if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
            // await _invitationRepository.DeleteAsync(res.Data.Id); // TODO voir si cela la delete sans cette ligne

            return Response.Ok("Successfully accepted invitation.");
        }
        return Response.BadRequest(res.Data.InvitationType == InvitationType.Event ? "Event must be provided." : "Workspace must be provided.", null);
        
    }

    public async Task<ActionResult> CancelAsync(string id, User user)
    {
        var res = await _invitationRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, "Invitation");
        if (res.Data.SenderId != user.Id) return Response.Unauthorized(); // TODO a changer par le droit

        if (res.Data.InvitationType == InvitationType.Event && res.Data.Event != null)
        {
            var eventUser = res.Data.Event.Users.Where(x => x.UserId == user.Id && x.DeletedAt == null).FirstOrDefault();
            if (eventUser == null) return Response.BadRequest(null, null);

            var res2 = await _eventRepository.UpdateAsync(res.Data.Event);
            if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        }
        var res3 = await _invitationRepository.DeleteAsync(id); 
        if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);

        return Response.Ok("Successfully canceled invitation.");
    }

    public async Task<ActionResult<InvitationDto>> CreateEventInvitationAsync(string senderId, string receiverId, string eventId)
    {
        var sender = await _userManager.FindByIdAsync(senderId);
        if (sender == null) return Response.Unauthorized();

        var receiver = await _userManager.FindByIdAsync(receiverId);
        if (receiver == null) return Response.NotFound(receiverId, "User");

        var res = await _eventRepository.GetEntityByIdAsync(eventId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(eventId, "Event");

        var res2 = await _invitationRepository.GetEntityByIdAsync(eventId + "_" + receiverId); // TODO a changer par une autre méthode
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        if (res.Data == null) return Response.BadRequest("User already invited.", null);

        var invitation = new Invitation(res.Data, sender, receiver) { Receiver = receiver, Sender = sender };

        var res3 = await _invitationRepository.CreateAsync(invitation);
        if (!res3.Success) return Response.BadRequest(res3.Message, res3.Details);

        return Response.CreatedAt(_mapper.Map<InvitationDto>(invitation));
    }

    public async Task<ActionResult<InvitationDto>> CreateWorkspaceInvitationAsync(string senderId, string receiverId, string workspaceId)
    {
        var sender = await _userManager.FindByIdAsync(senderId);
        if (sender == null) return Response.Unauthorized();

        var receiver = await _userManager.FindByIdAsync(receiverId);
        if (receiver == null) return Response.NotFound(receiverId, "User");

        var res = await _workspaceRepository.GetEntityByIdAsync(workspaceId);
        if (res.Success)
        {
            if (res.Data == null) return Response.NotFound(workspaceId, "Workspace");

            var existingInvitation = await _invitationRepository.GetEntityByIdAsync(workspaceId + "_" + receiverId);
            if (existingInvitation == null) return new BadRequestObjectResult("User already invited.");

            var invitation = new Invitation(res.Data, sender, receiver) { Receiver = receiver, Sender = sender };
            await _invitationRepository.CreateAsync(invitation);

            return Response.CreatedAt(_mapper.Map<InvitationDto>(invitation)); ;
        }
        return Response.BadRequest(res.Message, res.Details);
    }

    // public async Task<IActionResult> GetByIdAsync(string id)
    // {
    //     var invitation = await _invitationRepository.GetByIdAsync(id);
    //     if (invitation == null) return Response.NotFound(id, "Invitation");

    //     return Response.Ok(new InvitationDto(invitation));
    // }

    public async Task<ActionResult<List<InvitationDto>>> GetByReceiverUserAsync(User user)
    {
        var invitations = await _invitationRepository.GetByReceiverUserIdAsync(user.Id);

        return Response.Ok(invitations.Select(x => _mapper.Map<InvitationDto>(x)).ToList());
    }
}
