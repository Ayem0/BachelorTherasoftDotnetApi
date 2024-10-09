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
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParticipantCategoryDto?>> GetById(string id)
        {
            var res = await _participantCategoryService.GetByIdAsync(id);
            if (!res.Success) return NotFound(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Creates a ParticipantCategory.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParticipantCategoryDto>> Create([FromBody] CreateParticipantCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _participantCategoryService.CreateAsync(request.WorkspaceId, request.Name, request.Icon);
            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create),res.Content);
        }

        /// <summary>
        /// Deletes a ParticipantCategory.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _participantCategoryService.DeleteAsync(id);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Updates a ParticipantCategory.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateParticipantCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _participantCategoryService.UpdateAsync(id, request.NewName, request.NewIcon);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }
    }
}
