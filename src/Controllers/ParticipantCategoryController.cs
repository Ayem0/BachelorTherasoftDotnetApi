using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api")]
    [ApiController]
    public class ParticipantCategoryController : ControllerBase
    {
        private readonly IParticipantCategoryService _participantCategoryService;
        public ParticipantCategoryController(IParticipantCategoryService participantCategoryService)
        {
            _participantCategoryService = participantCategoryService;
        }

        /// <summary>
        /// Get a ParticipantCategory by id.
        /// </summary>
        [HttpGet("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var res = await _participantCategoryService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Creates a ParticipantCategory.
        /// </summary>
        [HttpPost("[controller]")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateParticipantCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _participantCategoryService.CreateAsync(request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Deletes a ParticipantCategory.
        /// </summary>
        [HttpDelete("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var res = await _participantCategoryService.DeleteAsync(id);
            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"Participant category with id '{id} not found.'" });
        }

        /// <summary>
        /// Updates a ParticipantCategory.
        /// </summary>
        [HttpPut("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateParticipantCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _participantCategoryService.UpdateAsync(id, request);
            return Ok(res);
        }

        /// <summary>
        /// Get participant categories.
        /// </summary>
        [HttpGet("Workspace/{workspaceId}/ParticipantCategories")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByWorkspaceId([FromRoute] string workspaceId)
        {
            var res = await _participantCategoryService.GetByWorkspaceIdAsync(workspaceId);
            return Ok(res);
        }
    }
}
