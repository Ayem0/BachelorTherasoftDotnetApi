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
        private readonly IMemberService _memberService;
        private readonly IWorkspaceService _workspaceService;
        public MemberController(IMemberService memberService, IWorkspaceService workspaceService)
        {
            _memberService = memberService;
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Get a Member by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto?>> GetById(string id)
        {
            var member = await _memberService.GetByIdAsync(id);

            return member != null ? Ok(member) : NotFound();
        }

        /// <summary>
        /// Creates a Member.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MemberDto>> Create([FromBody] CreateMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var member = await _memberService.CreateAsync(request.WorkspaceId, request.UserId);

            return member != null ? CreatedAtAction(nameof(Create), new { id = member.Id }, member) : BadRequest();
        }

        /// <summary>
        /// Deletes a Member.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _memberService.DeleteAsync(id);

            return res ? Ok() : BadRequest();
        }

        /// <summary>
        /// Updates a Member.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _memberService.UpdateAsync(id, request.NewStatus);

            return res != null ? Ok() : BadRequest();
        }
    }
}
