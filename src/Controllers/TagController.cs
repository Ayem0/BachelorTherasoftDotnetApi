using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/Workspace/{workspaceId}")]
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
        [HttpGet("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] string workspaceId, [FromRoute] string id)
        {
            var tag = await _tagService.GetByIdAsync(workspaceId, id);
            return Ok(tag);
        }

        /// <summary>
        /// Creates a Tag.
        /// </summary>
        [HttpPost("[controller]")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromRoute] string workspaceId, [FromBody] CreateTagRequest request)
        {
            var res = await _tagService.CreateAsync(workspaceId, request);
            return CreatedAtAction("Create", res);
        }

        /// <summary>
        /// Deletes a Tag.
        /// </summary>
        [HttpDelete("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string workspaceId, [FromRoute] string id)
        {
            var res = await _tagService.DeleteAsync(workspaceId, id);
            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"Tag with id '{id} not found.'" });
        }

        /// <summary>
        /// Updates a Tag.
        /// </summary>
        [HttpPut("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] string workspaceId, [FromRoute] string id, [FromBody] UpdateTagRequest request)
        {
            if (request.Name == null && request.Description == null && request.Color == null && request.Name == null) return BadRequest(new ProblemDetails() { Title = "At least one field is required." });
            var tag = await _tagService.UpdateAsync(workspaceId, id, request);
            return Ok(tag);
        }

        /// <summary>
        /// Get tags.
        /// </summary>
        [HttpGet("Tags")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByWorkspace([FromRoute] string workspaceId)
        {
            var tags = await _tagService.GetByWorkspaceIdAsync(workspaceId);
            return Ok(tags);
        }
    }
}
