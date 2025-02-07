using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO permettre de modifier l'area d'une room

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        /// Get a Room by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var res = await _roomService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Get rooms by area id.
        /// </summary>
        [HttpGet("area")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByAreaId([FromQuery] string areaId)
        {
            var res = await _roomService.GetByAreaIdAsync(areaId);
            return Ok(res);
        }

        /// <summary>
        /// Get rooms by workspace id.
        /// </summary>
        [HttpGet("workspace")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByWorkspaceId([FromQuery] string id)
        {
            var res = await _roomService.GetByWorkspaceIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Creates a Room.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _roomService.CreateAsync(request);
            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Deletes a Room.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var res = await _roomService.DeleteAsync(id);
            return res ? NoContent() : NotFound(new ProblemDetails() { Title = $"Room with id '{id} not found.'" });
        }

        /// <summary>
        /// Updates a Room.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _roomService.UpdateAsync(id, request);
            return Ok(res);
        }
    }
}
