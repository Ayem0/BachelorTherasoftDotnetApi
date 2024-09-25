using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult> Create([FromBody] CreateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspaceRole = await _workspaceRoleService.CreateAsync(request.Name, request.WorkspaceId);

            if ( workspaceRole == null) return BadRequest();
            
            return CreatedAtAction(nameof(Create), new { id = workspaceRole.Id }, workspaceRole);
        }

        /// <summary>
        /// Deletes a role from a workspace.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspaceRole = await _workspaceRoleService.DeleteAsync(id);

            if (workspaceRole) return Ok();

            return BadRequest();
        }

        [HttpPost("{id}/AddRoleToMember/{userId}")]
        [Authorize]
        public async Task<ActionResult> AddRoleToMember(string id, string userId)
        {
            var res = await _workspaceRoleService.AddRoleToMemberAsync(id, userId);

            if (res) return Ok();

            return BadRequest();
        }

        [HttpDelete("{id}/RemoveRoleFromMember/{userId}")]
        [Authorize]
        public async Task<ActionResult> RemoveRoleFromMember(string id, string userId)
        {
            var res = await _workspaceRoleService.AddRoleToMemberAsync(id, userId);
            
            if (res) return Ok();

            return BadRequest();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Update(string id, [FromBody] UpdateWorkspaceRoleRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var res = await _workspaceRoleService.UpdateAsync(id, request.NewName);

            if (res) return Ok();

            return BadRequest();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WorkspaceRoleDto?>> GetById(string id)
        {
            var role = await _workspaceRoleService.GetByIdAsync(id);

            if (role == null) return BadRequest();
            
            return Ok(role);
        }
    }
}
