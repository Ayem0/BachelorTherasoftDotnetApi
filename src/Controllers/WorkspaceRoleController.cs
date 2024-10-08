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
        public async Task<ActionResult> Create([FromBody] CreateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var workspaceRole = await _workspaceRoleService.CreateAsync(request.WorkspaceId, request.Name, request.Description);
            if (workspaceRole == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = workspaceRole.Id }, workspaceRole);
        }

        /// <summary>
        /// Deletes a role from a workspace.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(string id)
        {
            var res = await _workspaceRoleService.DeleteAsync(id);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Adds a workspace role to a member.
        /// </summary>
        [HttpPost("{id}/AddRoleToMember/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddRoleToMember(string id, string userId)
        {
            var res = await _workspaceRoleService.AddRoleToMemberAsync(id, userId);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Reomves a workspace role from a member.
        /// </summary>
        [HttpDelete("{id}/RemoveRoleFromMember/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveRoleFromMember(string id, string userId)
        {
            var res = await _workspaceRoleService.AddRoleToMemberAsync(id, userId);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Updates a workspace role.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(string id, [FromBody] UpdateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _workspaceRoleService.UpdateAsync(id, request.NewName, request.Description);
            if (res == null) return BadRequest();

            return Ok(res);
        }

        /// <summary>
        /// Gets a workspace role by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkspaceRoleDto?>> GetById(string id)
        {
            var role = await _workspaceRoleService.GetByIdAsync(id);
            if (role == null) return NotFound();

            return Ok(role);
        }
    }
}
