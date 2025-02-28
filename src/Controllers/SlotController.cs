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
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var res = await _SlotService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get a Slot by id.
        /// </summary>
        [HttpGet("workspace")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByWorkspaceId([FromQuery] string workspaceId)
        {
            var res = await _SlotService.GetByWorkpaceIdAsync(workspaceId);
            return Ok(res);
        }

        /// <summary>
        /// Creates a Slot.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSlotRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _SlotService.CreateAsync(request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Deletes a Slot.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var res = await _SlotService.DeleteAsync(id);
            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"Slot with id '{id} not found.'" });
        }


        /// <summary>
        /// Creates a Slot with repetition.
        /// </summary>
        [HttpPost("WithRepetition")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWithRepetition([FromBody] CreateSlotWithRepetitionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _SlotService.CreateWithRepetitionAsync(request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Add a Slot to a Room.
        /// </summary>
        [HttpPost("Room")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSlotToRoom([FromQuery] string slotId, [FromQuery] string roomId)
        {
            var res = await _SlotService.AddSlotToRoom(slotId, roomId);
            return res ? NoContent() : BadRequest();
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
