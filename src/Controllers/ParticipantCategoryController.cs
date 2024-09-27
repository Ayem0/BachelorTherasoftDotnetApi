using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParticipantCategoryDto?>> GetById(string id)
        {
            var ParticipantCategory = await _participantCategoryService.GetByIdAsync(id);

            if (ParticipantCategory == null) return NotFound();
  
            return Ok(ParticipantCategory);
        }

        /// <summary>
        /// Creates a ParticipantCategory.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParticipantCategoryDto>> Create([FromBody] CreateParticipantCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var ParticipantCategory = await _participantCategoryService.CreateAsync(request.WorkspaceId, request.Name, request.Icon);

            if (ParticipantCategory == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = ParticipantCategory.Id }, ParticipantCategory);
        }

        /// <summary>
        /// Deletes a ParticipantCategory.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var res = await _participantCategoryService.DeleteAsync(id);

            if (res) return Ok();

            return BadRequest();
        }
        
        /// <summary>
        /// Updates a ParticipantCategory.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateParticipantCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var res = await _participantCategoryService.UpdateAsync(id, request.NewName, request.NewIcon);

            if (res) return Ok();

            return BadRequest();
        }
    }
}
