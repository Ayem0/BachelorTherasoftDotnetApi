using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventMemberController : ControllerBase
    {
        private readonly IEventMemberService _eventMemberService;
        public EventMemberController(IEventMemberService eventMemberService)
        {
            _eventMemberService = eventMemberService;
        }
        
        /// <summary>
        /// Get a EventMember by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventMemberDto?>> GetById(string id)
        {
            var EventMember = await _eventMemberService.GetByIdAsync(id);

            return EventMember != null ? Ok(EventMember) : NotFound();
        }

        /// <summary>
        /// Creates a EventMember.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventMemberDto>> Create([FromBody] CreateEventMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var eventMember = await _eventMemberService.CreateAsync(request.EventId, request.MemberId);

            return eventMember != null ? CreatedAtAction(nameof(Create), eventMember) : BadRequest();
        }

        /// <summary>
        /// Deletes a EventMember.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _eventMemberService.DeleteAsync(id);

            return res ? Ok() : BadRequest();
        }

        /// <summary>
        /// Updates a EventMember.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateEventMemberRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _eventMemberService.UpdateAsync(id, request.NewStatus);

            return res != null ? Ok() : BadRequest();
        }
    }
}
