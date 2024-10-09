using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces;
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
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventDto?>> GetById(string id)
        {
            var res = await _eventService.GetByIdAsync(id);
            if (!res.Success) return NotFound(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Creates a Event.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _eventService.CreateAsync(request.Description, request.RoomId, request.EventCategoryId, request.StartDate, request.EndDate, request.ParticipantIds, request.TagIds);
            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create), res.Content);
        }

        /// <summary>
        /// Deletes a Event.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _eventService.DeleteAsync(id);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Updates a Event.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateEventRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _eventService.UpdateAsync(id, request.NewStartDate, request.NewEndDate, request.NewRoomId, request.NewDescription, request.NewEventCategoryId, request.NewParticipantIds, request.NewTagIds);
            if (res == null) return BadRequest();

            return Ok(res);
        }

        /// <summary>
        /// Creates a Event with repetition.
        /// </summary>
        [HttpPost("WithRepetition")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDto>> CreateWithRepetition([FromBody] CreateEventWithRepetitionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _eventService.CreateWithRepetitionAsync(request);
            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create), res.Content);
        }
    }
}
