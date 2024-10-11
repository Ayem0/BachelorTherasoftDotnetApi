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
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParticipantCategoryDto>> GetById([FromQuery] string id)
        {
            return await _participantCategoryService.GetByIdAsync(id);
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

            return await _participantCategoryService.CreateAsync(request.WorkspaceId, request.Name, request.Icon);
        }

        /// <summary>
        /// Deletes a ParticipantCategory.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            return await _participantCategoryService.DeleteAsync(id);
        }

        /// <summary>
        /// Updates a ParticipantCategory.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParticipantCategoryDto>> Update([FromQuery] string id, [FromBody] UpdateParticipantCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _participantCategoryService.UpdateAsync(id, request.NewName, request.NewIcon);
        }
    }
}
