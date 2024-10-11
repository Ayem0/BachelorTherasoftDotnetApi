using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
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
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SlotDto>> GetById([FromQuery] string id)
        {
            return await _SlotService.GetByIdAsync(id);
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

            return await _SlotService.CreateAsync(request);
        }

        /// <summary>
        /// Deletes a Slot.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            return await _SlotService.DeleteAsync(id);
        }


        /// <summary>
        /// Creates a Slot with repetition.
        /// </summary>
        [HttpPost("WithRepetition")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<SlotDto>>> CreateWithRepetition([FromBody] CreateSlotWithRepetitionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _SlotService.CreateWithRepetitionAsync(request);
        }

        /// <summary>
        /// Add a Slot to a Room.
        /// </summary>
        [HttpPost("Room")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSlotToRoom([FromQuery] string slotId,[FromQuery] string roomId)
        {
            return await _SlotService.AddSlotToRoom(slotId, roomId);
        }

        // TODO faire le update d'un slot / super chiant a faire car faut check pour chaque salle du créneaux si cela pose probleme
        // /// <summary>
        // /// Updates a Slot.
        // /// </summary>
        // [HttpPut("")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> Update(string id, [FromBody] UpdateSlotRequest request)
        // {
        //     if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

        //     return await _SlotService.UpdateAsync(id, request.NewName, request.NewIcon, request.NewDescription);
        //     if (res == null) return BadRequest();

        //     return Ok(res);
        // }
    }
}
