using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParticipantDto?>> GetById(string id)
        {
            var participant = await _participantService.GetByIdAsync(id);
            if (participant == null) return NotFound();

            return Ok(participant);
        }

        /// <summary>
        /// Creates a Participant.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParticipantDto>> Create([FromBody] CreateParticipantRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var participant = await _participantService.CreateAsync(request.WorkspaceId, request.ParticipantCategoryId, request.FirstName,
                request.LastName, request.Email, request.PhoneNumber, request.Description, request.Address, request.City, request.Country, request.DateOfBirth);
            if (participant == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = participant.Id }, participant);
        }

        /// <summary>
        /// Deletes a Participant.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _participantService.DeleteAsync(id);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Updates a Participant.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateParticipantRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _participantService.UpdateAsync(id, request.NewParticipantCategoryId, request.NewFirstName, request.NewLastName,
                request.NewEmail, request.NewDescription, request.NewAddress, request.NewCity, request.NewCountry, request.NewDateOfBirth);
            if (res == null) return BadRequest();

            return Ok(res);
        }
    }
}
