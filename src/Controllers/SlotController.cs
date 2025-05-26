using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api")]
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
        [HttpGet("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var res = await _SlotService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get slots.
        /// </summary>
        [HttpGet("Workspace/{workspaceId}/Slots")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByWorkspaceId([FromRoute] string workspaceId)
        {
            var res = await _SlotService.GetByWorkspaceIdAsync(workspaceId);
            return Ok(res);
        }

        /// <summary>
        /// Creates a Slot.
        /// </summary>
        [HttpPost("[controller]")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSlotRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _SlotService.CreateAsync(request);
            return CreatedAtAction(null, res);
        }



        /// <summary>
        /// Creates a Slot with repetition.
        /// </summary>
        [HttpPost("[controller]/WithRepetition")]
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
        /// Deletes a Slot.
        /// </summary>
        [HttpDelete("[controller]/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var res = await _SlotService.DeleteAsync(id);
            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"Slot with id '{id} not found.'" });
        }

        // /// <summary>
        // /// Add a Slot to a Room.
        // /// </summary>
        // [HttpPost("Room")]
        // [Authorize]
        // [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> AddSlotToRoom([FromQuery] string slotId, [FromQuery] string roomId)
        // {
        //     var res = await _SlotService.AddSlotToRoom(slotId, roomId);
        //     return res ? NoContent() : BadRequest();
        // }

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
