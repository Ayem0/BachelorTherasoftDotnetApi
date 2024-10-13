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
        public async Task<ActionResult<RoomDto>> GetById([FromQuery] string id)
        {
            return await _roomService.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a Room.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomDto>> Create([FromBody] CreateRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _roomService.CreateAsync(request);
        }

        /// <summary>
        /// Deletes a Room.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            return await _roomService.DeleteAsync(id);
        }

        /// <summary>
        /// Updates a Room.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomDto>> Update([FromQuery] string id, [FromBody] UpdateRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _roomService.UpdateAsync(id, request);
        }
    }
}
