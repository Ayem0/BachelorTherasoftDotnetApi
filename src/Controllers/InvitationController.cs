using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;
        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost("Workspace/Create")]
        [Authorize]
        public async Task<IActionResult> CreateWorkspaceInvitation([FromBody] CreateWorkspaceInvitationRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            if (userId == req.ReceiverUserId) return BadRequest();

            var invitation = await _invitationService.CreateWorkspaceInvitationAsync(userId, req);
            return CreatedAtAction(null, invitation);
        }

        [HttpPost("Workspace/{id}/Accept")]
        [Authorize]
        public async Task<IActionResult> AcceptWorkspaceInvitation(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            await _invitationService.AcceptWorkspaceInvitationAsync(userId, id);
            return Ok();
        }

        [HttpPost("Workspace/{id}/Cancel")]
        [Authorize]
        public async Task<IActionResult> CancelWorkspaceInvitation(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            await _invitationService.CancelWorkspaceInvitationAsync(userId, id);
            return Ok();
        }

        [HttpPost("Workspace/{id}/Refuse")]
        [Authorize]
        public async Task<IActionResult> RefuseWorkspaceInvitation(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            await _invitationService.RefuseWorkspaceInvitationAsync(userId, id);
            return Ok();
        }

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetByReceiverUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var invitations = await _invitationService.GetByReceiverUserIdAsync(userId);
            return Ok(invitations);
        }

        [HttpGet("Contact/Send")]
        [Authorize]
        public async Task<IActionResult> GetBySenderUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var invitations = await _invitationService.GetBySenderUserIdAsync(userId);
            return Ok(invitations);
        }

        [HttpGet("Workspace/{id}/Send")]
        [Authorize]
        public async Task<IActionResult> GetByWorkspaceId(string id)
        {
            var invitations = await _invitationService.GetByWorkspaceIdAsync(id);
            return Ok(invitations);
        }


        [HttpPost("Contact/{id}/Accept")]
        [Authorize]
        public async Task<IActionResult> AcceptContactInvitation(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            await _invitationService.AcceptContactInvitationAsync(userId, id);
            return Ok();
        }

        [HttpPost("Contact/Create")]
        [Authorize]
        public async Task<IActionResult> CreateContactInvitation([FromBody] CreateContactInvitationRequest req)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail != null && userEmail == req.ContactEmail) return BadRequest();
            if (userId == null) return Unauthorized();

            var invitation = await _invitationService.CreateContactInvitationAsync(userId, req.ContactEmail);
            return CreatedAtAction(null, invitation);
        }

        [HttpPost("Contact/{id}/Cancel")]
        [Authorize]
        public async Task<IActionResult> CancelContactInvitation(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            await _invitationService.CancelContactInvitationAsync(userId, id);
            return Ok();
        }

        [HttpPost("Contact/{id}/Refuse")]
        [Authorize]
        public async Task<IActionResult> RefuseContactInvitation(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            await _invitationService.RefuseContactInvitationAsync(userId, id);
            return Ok();
        }
    }
}
