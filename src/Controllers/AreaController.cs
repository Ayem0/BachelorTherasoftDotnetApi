using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO Ajouter la fonctionnalité d'update la location

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IWorkspaceService _workspaceService;
        public AreaController(IAreaService areaService, IWorkspaceService workspaceService)
        {
            _areaService = areaService;
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Get a Area by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AreaDto?>> GetById(string id)
        {
            var area = await _areaService.GetByIdAsync(id);

            if (!area.Success) return NotFound(area.Errors);
            
            return Ok(area.Content);
        }

        /// <summary>
        /// Creates a Area.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AreaDto>> Create([FromBody] CreateAreaRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var area = await _areaService.CreateAsync(request.LocationId, request.Name, request.Description);

            if (!area.Success) return BadRequest(area.Errors);
            
            return CreatedAtAction(nameof(Create), new { id = area.Content?.Id }, area.Content);
        }

        /// <summary>
        /// Deletes a Area.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _areaService.DeleteAsync(id);

            if (!res.Success) return BadRequest(res.Errors);
            
            return Ok(res.Content);
        }

        /// <summary>
        /// Updates a Area.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateAreaRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _areaService.UpdateAsync(id, request.NewName, request.NewDescription);

            if (!res.Success) return BadRequest(res.Errors);
            
            return Ok(res.Content);
        }
    }
}
