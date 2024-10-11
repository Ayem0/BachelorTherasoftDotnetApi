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
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventDto>> GetById([FromQuery] string id)
        {
            return await _eventService.GetByIdAsync(id);
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

            return await _eventService.CreateAsync(request.Description, request.RoomId, request.EventCategoryId, request.StartDate, request.EndDate, request.ParticipantIds, request.TagIds);
        }

        /// <summary>
        /// Deletes a Event.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            return await _eventService.DeleteAsync(id);
        }

        /// <summary>
        /// Updates a Event.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDto>> Update([FromQuery] string id, [FromBody] UpdateEventRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _eventService.UpdateAsync(id, request.NewStartDate, request.NewEndDate, request.NewRoomId, request.NewDescription, request.NewEventCategoryId, 
                request.NewParticipantIds, request.NewTagIds);
        }

        /// <summary>
        /// Creates a Event with repetition.
        /// </summary>
        [HttpPost("WithRepetition")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<EventDto>>> CreateWithRepetition([FromBody] CreateEventWithRepetitionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _eventService.CreateWithRepetitionAsync(request);
        }
    }
}
