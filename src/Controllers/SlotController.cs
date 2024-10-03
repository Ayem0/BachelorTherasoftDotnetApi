using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _SlotService;
        public SlotController(ISlotService SlotService)
        {
            _SlotService = SlotService;
        }

        /// <summary>
        /// Get a Slot by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SlotDto?>> GetById(string id)
        {
            var Slot = await _SlotService.GetByIdAsync(id);
            if (Slot == null) return NotFound();

            return Ok(Slot);
        }

        /// <summary>
        /// Creates a Slot.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SlotDto>> Create([FromBody] CreateSlotRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var Slot = await _SlotService.CreateAsync(request.WorkspaceId, request.StartDate, request.EndDate, request.StartTime, request.EndTime, request.EventCategoryIds);
            if (Slot == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = Slot.Id }, Slot);
        }

        /// <summary>
        /// Deletes a Slot.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _SlotService.DeleteAsync(id);
            if (!res) return BadRequest();

            return Ok();
        }


        /// <summary>
        /// Add a Slot to a Room.
        /// </summary>
        [HttpPost("AddSlot/{id}/ToRoom/{roomId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSlotToRoom(string id, string roomId)
        {
            var res = await _SlotService.AddSlotToRoom(id, roomId);
            if (!res) return BadRequest();

            return Ok();
        }

        // /// <summary>
        // /// Updates a Slot.
        // /// </summary>
        // [HttpPut("{id}")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> Update(string id, [FromBody] UpdateSlotRequest request)
        // {
        //     if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

        //     var res = await _SlotService.UpdateAsync(id, request.NewName, request.NewIcon, request.NewDescription);
        //     if (res == null) return BadRequest();

        //     return Ok(res);
        // }
    }
}
