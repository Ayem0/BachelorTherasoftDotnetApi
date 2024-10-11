using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        /// <summary>
        /// Get a Conversation by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ConversationDto?>> GetById([FromQuery] string id)
        {
            var conversation = await _conversationService.GetByIdAsync(id);

            return conversation != null ? Ok(conversation) : NotFound();
        }

        /// <summary>
        /// Get Conversations by user id.
        /// </summary>
        [HttpGet("Me")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ConversationDto>?>> GetByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var conversations = await _conversationService.GetByUserIdAsync(userId);

            return conversations != null ? Ok(conversations) : NotFound();
        }

        /// <summary>
        /// Creates a Conversation.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ConversationDto>> Create([FromBody] CreateConversationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var conversation = await _conversationService.CreateAsync(request.UserIds, request.Name);

            return conversation != null ? CreatedAtAction(nameof(Create), new { id = conversation.Id }, conversation) : BadRequest();
        }
    }
}
