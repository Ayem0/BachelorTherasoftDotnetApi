using BachelorTherasoftDotnetApi.src.Dtos;
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
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SlotDto?>> GetById(string id)
        {
            var res = await _SlotService.GetByIdAsync(id);
            if (!res.Success) return NotFound(res.Errors);

            return Ok(res.Content);
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

            var res = await _SlotService.CreateAsync(request);
            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create), res.Content);
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
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }


        /// <summary>
        /// Creates a Slot with repetition.
        /// </summary>
        [HttpPost("WithRepetition")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SlotDto>> CreateWithRepetition([FromBody] CreateSlotWithRepetitionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _SlotService.CreateWithRepetitionAsync(request);
            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create), res.Content);
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
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        // TODO faire le update d'un slot / super chiant a faire car faut check pour chaque salle du créneaux si cela pose probleme
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
