using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api")]
    [ApiController]
    public class WorkspaceRoleController : ControllerBase
    {
        private readonly IWorkspaceRoleService _workspaceRoleService;

        public WorkspaceRoleController(IWorkspaceRoleService workspaceRoleService)
        {
            _workspaceRoleService = workspaceRoleService;
        }

        /// <summary>
        /// Creates a role for a workspace.
        /// </summary>
        [HttpPost("[controller]")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _workspaceRoleService.CreateAsync(request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Deletes a role from a workspace.
        /// </summary>
        [HttpDelete("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var res = await _workspaceRoleService.DeleteAsync(id);
            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"WorkspaceRole with id '{id} not found.'" });

        }

        /// <summary>
        /// Updates a workspace role.
        /// </summary>
        [HttpPut("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            if (request.Description == null && request.Name == null) return BadRequest(new ProblemDetails() { Title = "At least one field is required." });

            var res = await _workspaceRoleService.UpdateAsync(id, request);
            return Ok(res);
        }

        /// <summary>
        /// Gets a workspace role by id.
        /// </summary>
        [HttpGet("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var res = await _workspaceRoleService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Gets workspace roles by workspace id.
        /// </summary>
        [HttpGet("Workspace/{workspaceId}/WorkspaceRoles")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByWorkspaceId([FromRoute] string workspaceId)
        {
            var res = await _workspaceRoleService.GetByWorkspaceIdAsync(workspaceId);
            return Ok(res);
        }

        // /// <summary>
        // /// Adds a workspace role to a member.
        // /// </summary>
        // [HttpPost("Member")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<ActionResult> AddRoleToMember([FromQuery] string workspaceRoleId, [FromQuery] string memberId)
        // {
        //     var res =  await _workspaceRoleService.(workspaceRoleId, memberId);
        // }

        // /// <summary>
        // /// Reomves a workspace role from a member.
        // /// </summary>
        // [HttpDelete("Member")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<ActionResult> RemoveRoleFromMember([FromQuery] string workspaceRoleId, [FromQuery] string memberId)
        // {
        //     return await _workspaceRoleService.AddRoleToMemberAsync(workspaceRoleId, memberId);
        // }

    }
}
