using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var res = await _workspaceService.GetByIdAsync(id);
            return Ok(res);
        }


        /// <summary>
        /// Get a workspace by id.
        /// </summary>
        [HttpGet("User")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUser()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            
            var res = await _workspaceService.GetByUserIdAsync(userId);
            return Ok(res);
        }

         /// <summary>
        /// Get a workspace with details by id.
        /// </summary>
        [HttpGet("Details")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailsById([FromQuery] string id)
        {
            var res = await _workspaceService.GetDetailsByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Creates a workspace.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateWorkspaceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Forbid();

            var res = await _workspaceService.CreateAsync(userId, request);
            return CreatedAtAction(null, res);
        }

        // /// <summary>
        // /// Removes a member from a workspace.
        // /// </summary>
        // [HttpDelete("Member")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<ActionResult> RemoveMember([FromQuery] string workspaceId, [FromQuery] string memberId)
        // {
        //     return await _workspaceService.RemoveMemberAsync(workspaceId, memberId);
        // }

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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var res = await _workspaceService.DeleteAsync(id);

            return res ? NoContent(): NotFound(new ProblemDetails() { Title = $"Workspace with id '{id} not found.'"});
        }

        /// <summary>
        /// Updates a workspace.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateWorkspaceRequest request)
        {
            if (request.NewName == null && request.NewDescription == null) return BadRequest(new ProblemDetails() { Title = "At least one field is required."});

            var res = await _workspaceService.UpdateAsync(id, request);

            return Ok(res);
        }
    }
}
