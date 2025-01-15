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
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        /// <summary>
        /// Get a Participant by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var res =  await _participantService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Creates a Participant.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateParticipantRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _participantService.CreateAsync(request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Deletes a Participant.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var res = await _participantService.DeleteAsync(id);
            return res ? NoContent(): NotFound(new ProblemDetails() { Title = $"Participant with id '{id} not found.'"});
        }

        /// <summary>
        /// Updates a Participant.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateParticipantRequest req)
        {
            if (req.Address == null && req.FirstName == null && req.LastName == null && req.City == null && req.DateOfBirth == null 
                && req.Country == null && req.Email == null && req.ParticipantCategoryId == null && req.Description == null)
                return BadRequest(new ProblemDetails() { Title = "At least one field is required."});

            var res = await _participantService.UpdateAsync(id, req);

            return Ok(res);
        }
    }
}
