using BachelorTherasoftDotnetApi.src.Dtos;
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
            var res = await _eventCategoryService.GetByIdAsync(id);

            if (!res.Success) return NotFound(res.Errors);

            return Ok(res.Content);
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

            var res = await _eventCategoryService.CreateAsync(request.WorkspaceId, request.Name, request.Icon, request.Color);

            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create), new { id = res.Content?.Id }, res.Content);
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

            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);

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
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }
    }
}
