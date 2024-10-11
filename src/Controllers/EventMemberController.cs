// using BachelorTherasoftDotnetApi.src.Dtos.Create;
// using BachelorTherasoftDotnetApi.src.Dtos.Models;
// using BachelorTherasoftDotnetApi.src.Dtos.Update;
// using BachelorTherasoftDotnetApi.src.Interfaces.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace BachelorTherasoftDotnetApi.src.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class EventMemberController : ControllerBase
//     {
//         private readonly IEventMemberService _eventMemberService;
//         public EventMemberController(IEventMemberService eventMemberService)
//         {
//             _eventMemberService = eventMemberService;
//         }
        
//         /// <summary>
//         /// Get a EventMember by id.
//         /// </summary>
//         [HttpGet("")]
//         [Authorize]
//         [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
//         public async Task<ActionResult<EventMemberDto?>> GetById([FromQuery] string id)
//         {
//             var res = await _eventMemberService.GetByIdAsync(id);

//             if (!res.Success) return NotFound(res.Errors);
 
//             return Ok(res.Content);
//         }

//         /// <summary>
//         /// Creates a EventMember.
//         /// </summary>
//         [HttpPost("")]
//         [Authorize]
//         [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
//         public async Task<ActionResult<EventMemberDto>> Create([FromBody] CreateEventMemberRequest request)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

//             var res = await _eventMemberService.CreateAsync(request.EventId, request.MemberId);

//             if (!res.Success) return BadRequest(res.Errors);
 
//             return CreatedAtAction(nameof(Create), res.Content);
//         }

//         /// <summary>
//         /// Deletes a EventMember.
//         /// </summary>
//         [HttpDelete("")]
//         [Authorize]
//         [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
//         public async Task<IActionResult> Delete([FromQuery] string id)
//         {
//             var res = await _eventMemberService.DeleteAsync(id);
            
//             if (!res.Success) return BadRequest(res.Errors);

//             return Ok(res.Content);
//         }

//         /// <summary>
//         /// Updates a EventMember.
//         /// </summary>
//         [HttpPut("")]
//         [Authorize]
//         [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
//         public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateEventMemberRequest request)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

//             var res = await _eventMemberService.UpdateAsync(id, request.NewStatus);

//             if (!res.Success) return BadRequest(res.Errors);

//             return Ok(res.Content);
//         }
//     }
// }
