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
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AreaDto>> GetById([FromQuery] string id)
        {
            return await _areaService.GetByIdAsync(id);
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

            return await _areaService.CreateAsync(request.LocationId, request.Name, request.Description);
        }

        /// <summary>
        /// Deletes a Area.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromQuery] string id)
        {
            return await _areaService.DeleteAsync(id);
        }

        /// <summary>
        /// Updates a Area.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AreaDto>> Update([FromQuery] string id, [FromBody] UpdateAreaRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _areaService.UpdateAsync(id, request.NewName, request.NewDescription);
        }
    }
}
