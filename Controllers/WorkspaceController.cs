using System.Security.Claims;
using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Enums;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetWorkspaceById(string id)
        {
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(id);
            if (workspace == null)
            {
                return NotFound();
            }
            return Ok(new WorkspaceDto{
                Id = workspace.Id,
                Name = workspace.Name,
                Users = workspace.Users.Select(user => new UserDto {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }).ToList() 
            });
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return BadRequest("User not found");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return BadRequest("User not found");

            var workspace = new Workspace{
                Name = request.Name,
                Users = [user]
            };

            await _workspaceService.CreateWorkspaceAsync(workspace);

            return Ok(new WorkspaceDto{
                Id = workspace.Id,
                Name = workspace.Name,
            });
        }

        [HttpPost("RemoveMember")]
        [Authorize]
        public async Task<IActionResult> RemoveMember([FromBody] RemoveMemberRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            

            var workspace = await _workspaceService.GetWorkspaceByIdAsync(request.WorkspaceId);

            if (workspace == null) return NotFound();

            var user = await _userManager.FindByIdAsync(request.MemberId);

            if (user == null) return NotFound();

            if (workspace.Users.Contains(user)) { // A changer par si l'user a le droit de remove des membres 
                workspace.Users.Remove(user);
                await _workspaceService.UpdateWorkspaceAsync(workspace);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("AddMember")]
        [Authorize]
        public async Task<IActionResult> AddMember([FromBody] RemoveMemberRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(request.WorkspaceId);

            if (workspace == null) return NotFound();

            var user = await _userManager.FindByIdAsync(request.MemberId);

            if (user == null) return NotFound();

            if (!workspace.Users.Contains(user)) {
                workspace.Users.Add(user);
                await _workspaceService.UpdateWorkspaceAsync(workspace);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] DeleteWorkspaceRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var workspace = await _workspaceService.GetWorkspaceByIdAsync(request.Id);

            if (workspace == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return NotFound();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            if (workspace.Users.Contains(user)) { //           A CHANGER !!! SI L'USER A LE UN ROLE QUI A LE DROIT DE DELETE ALORS OUI SINON UNAUTHORIZED
                await _workspaceService.DeleteWorkspaceAsync(workspace.Id);
                return Ok();
            }

            return Unauthorized();
        }
    }
}
