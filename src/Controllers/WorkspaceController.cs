using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces;
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
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkspaceDto>> GetById(string id)
        {
            var workspace = await _workspaceService.GetByIdAsync(id);
            if (workspace == null) return NotFound();

            return Ok(workspace);
        }

        /// <summary>
        /// Creates a workspace.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest / StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WorkspaceDto>> Create([FromBody] CreateWorkspaceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var workspace = await _workspaceService.CreateAsync(userId, request.Name, request.Description);
            if (workspace == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = workspace.Id }, workspace);
        }

        /// <summary>
        /// Removes a member from a workspace.
        /// </summary>
        [HttpDelete("{id}/RemoveMember/{memberId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveMember(string id, string memberId)
        {
            var res = await _workspaceService.RemoveMemberAsync(id, memberId);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Adds a member to a workspace.
        /// </summary>
        [HttpPost("{id}/AddMember/{memberId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMember(string id, string memberId)
        {
            var res = await _workspaceService.AddMemberAsync(id, memberId);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Deletes a workspace.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _workspaceService.DeleteAsync(id);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Updates a workspace.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateWorkspaceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _workspaceService.UpdateAsync(id, request.NewName, request.Description);
            if (res == null) return BadRequest();

            return Ok(res);
        }
    }
}
