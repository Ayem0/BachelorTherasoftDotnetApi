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
        public async Task<ActionResult> Create([FromBody] CreateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _workspaceRoleService.CreateAsync(request);
            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create), res.Content);
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
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Adds a workspace role to a member.
        /// </summary>
        [HttpPost("{id}/AddToMember/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddRoleToMember(string id, string userId)
        {
            var res = await _workspaceRoleService.AddRoleToMemberAsync(id, userId);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Reomves a workspace role from a member.
        /// </summary>
        [HttpDelete("{id}/RemoveFromMember/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemoveRoleFromMember(string id, string userId)
        {
            var res = await _workspaceRoleService.AddRoleToMemberAsync(id, userId);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
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

            var res = await _workspaceRoleService.UpdateAsync(id, request);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Gets a workspace role by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkspaceRoleDto?>> GetById(string id)
        {
            var res = await _workspaceRoleService.GetByIdAsync(id);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }
    }
}
