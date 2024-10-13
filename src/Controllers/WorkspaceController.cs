using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;
        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Get a workspace by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkspaceDto>> GetById([FromQuery] string id)
        {
            return await _workspaceService.GetByIdAsync(id);
        }

         /// <summary>
        /// Get a workspace with details by id.
        /// </summary>
        [HttpGet("Details")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkspaceDetailsDto>> GetDetailsById([FromQuery] string id)
        {
            return await _workspaceService.GetDetailsByIdAsync(id);
        }

        /// <summary>
        /// Creates a workspace.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WorkspaceDto>> Create([FromBody] CreateWorkspaceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            return await _workspaceService.CreateAsync(userId, request);
        }

        /// <summary>
        /// Removes a member from a workspace.
        /// </summary>
        [HttpDelete("Member")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveMember([FromQuery] string workspaceId, [FromQuery] string memberId)
        {
            return await _workspaceService.RemoveMemberAsync(workspaceId, memberId);
        }

        // /// <summary>
        // /// Adds a member to a workspace.
        // /// </summary>
        // [HttpPost("{id}/AddMember/{memberId}")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> AddMember(string id, string memberId)
        // {
        //     return await _workspaceService.AddMemberAsync(id, memberId);
        // }

        /// <summary>
        /// Deletes a workspace.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            return await _workspaceService.DeleteAsync(id);
        }

        /// <summary>
        /// Updates a workspace.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkspaceDto>> Update([FromQuery] string id, [FromBody] UpdateWorkspaceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _workspaceService.UpdateAsync(id, request);
        }
    }
}
