using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
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
        private readonly IUserService _userService;
        public WorkspaceController(IWorkspaceService workspaceService, IUserService userService)
        {
            _workspaceService = workspaceService;
            _userService = userService;
        }

        /// <summary>
        /// Get a workspace by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {

            var res = await _workspaceService.GetByIdAsync(id);
            return Ok(res);
        }



        /// <summary>
        /// Get user workspaces
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetWorkspaces()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var workspaces = await _workspaceService.GetByUserIdAsync(userId);

            return Ok(workspaces);
        }

        /// <summary>
        /// Get workspace users.
        /// </summary>
        [HttpGet("{id}/Users")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers([FromRoute] string id)
        {
            var res = await _userService.GetByWorkspaceIdAsync(id);
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
            if (userId == null) return Unauthorized();

            var res = await _workspaceService.CreateAsync(userId, request);

            return CreatedAtAction(null, res);
        }


        /// <summary>
        /// Deletes a workspace.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var res = await _workspaceService.DeleteAsync(id);

            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"Workspace with id '{id} not found.'" });
        }

        /// <summary>
        /// Updates a workspace.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateWorkspaceRequest request)
        {
            if (request.Name == null && request.Description == null) return BadRequest(new ProblemDetails() { Title = "At least one field is required." });

            var res = await _workspaceService.UpdateAsync(id, request);
            return Ok(res);
        }




        // /// <summary>
        // /// Removes a member from a workspace.
        // /// </summary>
        // [HttpDelete("Member")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<ActionResult> RemoveMember([FromQuery] string workspaceworkspaceId, [FromQuery] string memberworkspaceId)
        // {
        //     return await _workspaceService.RemoveMemberAsync(workspaceworkspaceId, memberworkspaceId);
        // }

        // /// <summary>
        // /// Adds a member to a workspace.
        // /// </summary>
        // [HttpPost("{workspaceId}/AddMember/{memberworkspaceId}")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> AddMember(string workspaceId, string memberworkspaceId)
        // {
        //     return await _workspaceService.AddMemberAsync(workspaceId, memberworkspaceId);
        // }

    }
}
