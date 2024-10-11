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
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkspaceRoleDto>> Create([FromBody] CreateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _workspaceRoleService.CreateAsync(request);
        }

        /// <summary>
        /// Deletes a role from a workspace.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest / StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            return await _workspaceRoleService.DeleteAsync(id);
        }

        /// <summary>
        /// Adds a workspace role to a member.
        /// </summary>
        [HttpPost("Member")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddRoleToMember([FromQuery] string workspaceRoleId, [FromQuery] string memberId)
        {
            return await _workspaceRoleService.AddRoleToMemberAsync(workspaceRoleId, memberId);
        }

        /// <summary>
        /// Reomves a workspace role from a member.
        /// </summary>
        [HttpDelete("Member")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveRoleFromMember([FromQuery] string workspaceRoleId, [FromQuery] string memberId)
        {
            return await _workspaceRoleService.AddRoleToMemberAsync(workspaceRoleId, memberId);
        }

        /// <summary>
        /// Updates a workspace role.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkspaceRoleDto>> Update([FromQuery] string id, [FromBody] UpdateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _workspaceRoleService.UpdateAsync(id, request);
        }

        /// <summary>
        /// Gets a workspace role by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkspaceRoleDto>> GetById([FromQuery] string id)
        {
            return await _workspaceRoleService.GetByIdAsync(id);
        }
    }
}
