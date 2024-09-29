using BachelorTherasoftDotnetApi.src.Dtos;
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
            var Area = await _areaService.GetByIdAsync(id);

            return Area != null ? Ok(Area) : NotFound();
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

            var Area = await _areaService.CreateAsync(request.LocationId, request.Name, request.Description);

            return Area != null ? CreatedAtAction(nameof(Create), new { id = Area.Id }, Area) : BadRequest();
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

            return res ? Ok() : BadRequest();
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

            return res != null ? Ok(res) : BadRequest();
        }
    }
}
