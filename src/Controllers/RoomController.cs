using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO permettre de modifier l'area d'une room

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _RoomService;
        private readonly IWorkspaceService _workspaceService;
        public RoomController(IRoomService RoomService, IWorkspaceService workspaceService)
        {
            _RoomService = RoomService;   
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Get a Room by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto?>> GetById(string id)
        {
            var Room = await _RoomService.GetByIdAsync(id);

            if (Room == null) return NotFound();
  
            return Ok(Room);
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
            
            var Room = await _RoomService.CreateAsync(request.Name, request.AreaId, request.Description);

            if (Room == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = Room.Id }, Room);
        }

        /// <summary>
        /// Deletes a Room.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var res = await _RoomService.DeleteAsync(id);

            if (res) return Ok();

            return BadRequest();
        }
        
        /// <summary>
        /// Updates a Room.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());
            
            var res = await _RoomService.UpdateAsync(id, request.NewName, request.NewDescription);

            if (res) return Ok();

            return BadRequest();
        }
    }
}
