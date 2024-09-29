using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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
        private readonly IWorkspaceService _workspaceService;
        public RoomController(IRoomService roomService, IWorkspaceService workspaceService)
        {
            _roomService = roomService;
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Get a Room by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto?>> GetById(string id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();

            return Ok(room);
        }

        /// <summary>
        /// Creates a Room.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomDto>> Create([FromBody] CreateRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var room = await _roomService.CreateAsync(request.Name, request.AreaId, request.Description);
            if (room == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = room.Id }, room);
        }

        /// <summary>
        /// Deletes a Room.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _roomService.DeleteAsync(id);
            if (!res) return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Updates a Room.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _roomService.UpdateAsync(id, request.NewName, request.NewDescription);
            if (res == null) return BadRequest();

            return Ok(res);
        }
    }
}
