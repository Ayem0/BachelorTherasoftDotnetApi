using System.Security.Claims;
using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly UserManager<User> _userManager;
        public WorkspaceController(IWorkspaceService workspaceService, UserManager<User> userManager)
        {
            _workspaceService = workspaceService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get a workspace by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkspaceDto>> GetWorkspaceById(string id)
        {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);

            if (workspace == null)
            {
                return NotFound();
            }

            var workspaceDto = new WorkspaceDto{
                Id = workspace.Id,
                Name = workspace.Name,
                Users = workspace.Users.Select(user => new UserDto {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }).ToList() 
            };

            return Ok(workspaceDto);
        }

        /// <summary>
        /// Creates a workspace.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Create([FromBody] CreateWorkspaceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            var workspace = new Workspace{
                Name = request.Name,
                Users = [user]
            };

            await _workspaceService.CreateWorkspaceAsync(workspace);

            var workspaceDto = new WorkspaceDto{
                Id = workspace.Id,
                Name = workspace.Name,
            };

            return CreatedAtAction(nameof(Create), new { id = workspaceDto.Id }, workspaceDto);
        }

        /// <summary>
        /// Removes a member from a workspace.
        /// </summary>
        [HttpDelete("{id}/RemoveMember/{memberId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveMember(string id, string memberId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);

            if (workspace == null) return NotFound();

            var user = await _userManager.FindByIdAsync(memberId);

            if (user == null) return NotFound();

            if (workspace.Users.Contains(user)) { // A changer par si l'user a le droit de remove des membres 
                workspace.Users.Remove(user);
                await _workspaceService.UpdateWorkspaceAsync(workspace);
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Adds a member to a workspace.
        /// </summary>
        [HttpPost("{id}/AddMember/{memberId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMember(string id, string memberId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);

            if (workspace == null) return NotFound();

            var user = await _userManager.FindByIdAsync(memberId);

            if (user == null) return NotFound();

            if (!workspace.Users.Contains(user)) {
                workspace.Users.Add(user);
                await _workspaceService.UpdateWorkspaceAsync(workspace);
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Deletes a workspace.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);

            if (workspace == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            if (workspace.Users.Contains(user)) { //           A CHANGER !!! SI L'USER A LE UN ROLE QUI A LE DROIT DE DELETE ALORS OUI SINON UNAUTHORIZED
                await _workspaceService.DeleteWorkspaceAsync(workspace.Id);
                return Ok();
            }

            return BadRequest();
        }
        
        /// <summary>
        /// Updates a workspace.
        /// </summary>
        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateWorkspaceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);

            if (workspace == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            if (workspace.Users.Contains(user)) { 
                workspace.Name = request.NewName; // A CHANGER !!! SI L'USER A LE UN ROLE QUI A LE DROIT DE DELETE ALORS OUI SINON UNAUTHORIZED
                await _workspaceService.UpdateWorkspaceAsync(workspace);
                return Ok();
            }
            return Unauthorized();
        }
    }
}
