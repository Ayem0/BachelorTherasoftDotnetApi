using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class EventCategoryController : ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService;
        public EventCategoryController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
        }

        /// <summary>
        /// Get a EventCategory by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventCategoryDto?>> GetById(string id)
        {
            var EventCategory = await _eventCategoryService.GetByIdAsync(id);

            return EventCategory != null ? Ok(EventCategory) : NotFound();
        }

        /// <summary>
        /// Creates a EventCategory.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventCategoryDto>> Create([FromBody] CreateEventCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var EventCategory = await _eventCategoryService.CreateAsync(request.WorkspaceId, request.Name, request.Icon, request.Color);

            return EventCategory != null ? CreatedAtAction(nameof(Create), new { id = EventCategory.Id }, EventCategory) : BadRequest();
        }

        /// <summary>
        /// Deletes a EventCategory.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _eventCategoryService.DeleteAsync(id);

            return res ? Ok() : BadRequest();
        }

        /// <summary>
        /// Updates a EventCategory.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateEventCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _eventCategoryService.UpdateAsync(id, request.NewName, request.NewIcon);
            if (res == null) return BadRequest();

            return Ok(res);
        }
    }
}
