using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Get a Tag by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TagDto?>> GetById(string id)
        {
            var res = await _tagService.GetByIdAsync(id);
            if (!res.Success) return NotFound(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Creates a Tag.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TagDto>> Create([FromBody] CreateTagRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _tagService.CreateAsync(request);
            if (!res.Success) return NotFound(res.Errors);

            return CreatedAtAction(nameof(Create), res.Content);
        }

        /// <summary>
        /// Deletes a Tag.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _tagService.DeleteAsync(id);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Updates a Tag.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTagRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _tagService.UpdateAsync(id, request);
           if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }
    }
}
