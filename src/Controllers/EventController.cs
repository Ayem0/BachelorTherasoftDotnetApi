using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        /// <summary>
        /// Get a Event by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var res = await _eventService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Creates a Event.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var res = await _eventService.CreateAsync(userId, request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Deletes a Event.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var res = await _eventService.DeleteAsync(id);
            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"Event with id '{id} not found.'" });
        }

        /// <summary>
        /// Updates a Event.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateEventRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _eventService.UpdateAsync(id, request);
            return Ok(res);
        }

        /// <summary>
        /// Creates a Event with repetition.
        /// </summary>
        [HttpPost("WithRepetition")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWithRepetition([FromBody] CreateEventWithRepetitionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _eventService.CreateWithRepetitionAsync(request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Get events by range and room id.
        /// </summary>
        [HttpGet("room")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByRoomId([FromQuery] string id, [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var res = await _eventService.GetByRangeAndRoomIdAsync(id, start, end);
            return Ok(res);
        }

        /// <summary>
        /// Get events by range and user id.
        /// </summary>
        [HttpGet("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUser([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var res = await _eventService.GetByRangeAndUserIdAsync(userId, start, end);
            return Ok(res);
        }
    }
}
