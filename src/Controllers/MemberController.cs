// using BachelorTherasoftDotnetApi.src.Dtos;
// using BachelorTherasoftDotnetApi.src.Dtos.Create;
// using BachelorTherasoftDotnetApi.src.Dtos.Models;
// using BachelorTherasoftDotnetApi.src.Dtos.Update;
// using BachelorTherasoftDotnetApi.src.Interfaces.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace BachelorTherasoftDotnetApi.src.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class MemberController : ControllerBase
//     {
//         private readonly IMemberService _memberService;
//         private readonly IWorkspaceService _workspaceService;
//         public MemberController(IMemberService memberService, IWorkspaceService workspaceService)
//         {
//             _memberService = memberService;
//             _workspaceService = workspaceService;
//         }

//         /// <summary>
//         /// Get a Member by id.
//         /// </summary>
//         [HttpGet("")]
//         [Authorize]
//         [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
//         public async Task<ActionResult<MemberDto?>> GetById([FromQuery] string id)
//         {
//             var res = await _memberService.GetByIdAsync(id);
//             if (!res.Success) return NotFound(res.Errors);
            
//             return Ok(res.Content);
//         }

//         /// <summary>
//         /// Creates a Member.
//         /// </summary>
//         [HttpPost("")]
//         [Authorize]
//         [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
//         public async Task<ActionResult<MemberDto>> Create([FromBody] CreateMemberRequest request)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

//             var res = await _memberService.CreateAsync(request.WorkspaceId, request.UserId);
//             if (!res.Success) return BadRequest(res.Errors);

//             return CreatedAtAction(nameof(Create), res.Content);
//         }

//         /// <summary>
//         /// Deletes a Member.
//         /// </summary>
//         [HttpDelete("")]
//         [Authorize]
//         [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
//         public async Task<IActionResult> Delete([FromQuery] string id)
//         {
//             var res = await _memberService.DeleteAsync(id);
//             if (!res.Success) return BadRequest(res.Errors);

//             return Ok(res.Content);
//         }

//         // /// <summary>
//         // /// Updates a Member.
//         // /// </summary>
//         // [HttpPut("")]
//         // [Authorize]
//         // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
//         // public async Task<IActionResult> Update(string id, [FromBody] UpdateMemberRequest request)
//         // {
//         //     if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

//         //     var res = await _memberService.UpdateAsync(id, request.NewStatus);
//         //     if (!res.Success) return BadRequest(res.Errors);
//         //     return Ok(res.Content);
//         // }
//     }
// }
