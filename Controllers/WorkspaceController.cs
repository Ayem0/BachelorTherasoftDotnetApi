using System.Security.Claims;
using BachelorTherasoftDotnetApi.Dtos;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            }

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
                Users = [
                    new UserDto{
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    },
                ]
            });
        }
    }
}
