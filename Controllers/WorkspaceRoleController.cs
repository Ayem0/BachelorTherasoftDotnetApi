// using BachelorTherasoftDotnetApi.Dtos;
// using BachelorTherasoftDotnetApi.Interfaces;
// using BachelorTherasoftDotnetApi.Models;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace BachelorTherasoftDotnetApi.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class WorkspaceRoleController : ControllerBase
//     {
//         private readonly IWorkspaceRoleService _workspaceRoleService;
//         private readonly IWorkspaceService _workspaceService;
        
//         public WorkspaceRoleController(IWorkspaceRoleService workspaceRoleService, IWorkspaceService workspaceService)
//         {
//             _workspaceRoleService = workspaceRoleService;
//             _workspaceService = workspaceService;
//         }

//         /// <summary>
//         /// Creates a role for a workspace.
//         /// </summary>
//         [HttpPost("")]
//         [Authorize]
//         public async Task<ActionResult> Create([FromBody] CreateWorkspaceRoleRequest request)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
//             var workspace = await _workspaceService.GetWorkspaceByIdAsync(request.WorkspaceId);

//             if ( workspace == null) return NotFound();

//             var workspaceRole = new WorkspaceRole{
//                 Name = request.Name,
//                 Workspace = workspace,
//                 WorkspaceId = workspace.Id,
//             };

//             await _workspaceRoleService.CreateWorkspaceRoleAsync(workspaceRole);

//             var workspaceRoleDto = new WorkspaceRoleDto{
//                 Id = workspaceRole.Id,
//                 Name = workspaceRole.Name
//             };
            
//             return CreatedAtAction(nameof(Create), new { id = workspaceRoleDto.Id }, workspaceRoleDto);
//         }

//         /// <summary>
//         /// Creates a role for a workspace.
//         /// </summary>
//         [HttpPost("")]
//         [Authorize]
//         public async Task<ActionResult> Delete([FromBody] DeleteWorkspaceRoleRequest request)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
//             var role = await _workspaceRoleService.GetWorkspaceRoleByIdAsync(request.Id);

//             if ( role == null) return NotFound();

//             await _workspaceRoleService.DeleteWorkspaceRoleAsync(role.Id);

//             await _workspaceRoleService.CreateWorkspaceRoleAsync(workspaceRole);

//             var workspaceRoleDto = new WorkspaceRoleDto{
//                 Id = workspaceRole.Id,
//                 Name = workspaceRole.Name
//             };
            
//             return CreatedAtAction(nameof(CreateWorkspaceRole), new { id = workspaceRoleDto.Id }, workspaceRoleDto);
//         }



//     }
// }
