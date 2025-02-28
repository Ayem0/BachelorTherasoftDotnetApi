using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;
        public MemberController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Get members by workspace id.
        /// </summary>
        [HttpGet("Workspace/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByWorkspaceId(string id)
        {
            var res = await _workspaceService.GetMembersByIdAsync(id);

            return Ok(res);
        }

        // TODO faire l'update et le delete
        // /// <summary>
        // /// Deletes a Member.
        // /// </summary>
        // [HttpDelete("")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> Remove([FromQuery] string id)
        // {
        //     var res = await _memberService.DeleteAsync(id);
        //     if (!res.Success) return BadRequest(res.Errors);

        //     return Ok(res.Content);
        // }

        // /// <summary>
        // /// Updates a Member.
        // /// </summary>
        // [HttpPut("")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> Update(string id, [FromBody] UpdateMemberRequest request)
        // {
        //     if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

        //     var res = await _memberService.UpdateAsync(id, request.NewStatus);
        //     if (!res.Success) return BadRequest(res.Errors);
        //     return Ok(res.Content);
        // }
    }
}
