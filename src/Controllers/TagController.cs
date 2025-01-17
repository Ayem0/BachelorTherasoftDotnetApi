using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Get a Tag by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [WorkspaceAuthorize("Tag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromQuery] string id, [FromQuery] string workspaceId)
        {
            var tag = await _tagService.GetByIdAsync(id);
            return Ok(tag);
        }

        /// <summary>
        /// Creates a Tag.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request, [FromQuery] string workspaceId)
        {
            var res = await _tagService.CreateAsync(request);
            return CreatedAtAction("Create", res);
        }

        /// <summary>
        /// Deletes a Tag.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [WorkspaceAuthorize("Tag")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] string id, [FromQuery] string workspaceId)
        {
            var res = await _tagService.DeleteAsync(id);
            return res ? NoContent(): NotFound(new ProblemDetails() { Title = $"Tag with id '{id} not found.'"});
        }

        /// <summary>
        /// Updates a Tag.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [WorkspaceAuthorize("Tag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateTagRequest request)
        {
            if (request.Name == null && request.Description == null && request.Color == null && request.Name == null) return BadRequest(new ProblemDetails(){ Title = "At least one field is required."});
            var tag = await _tagService.UpdateAsync(id, request);
            return Ok(tag);
        }
    }
}
