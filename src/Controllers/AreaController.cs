using System.Security.Claims;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// TODO Ajouter la fonctionnalité d'update la location

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;
        public AreaController(IAreaService areaService, ILocationService locationService, IUserService userService)
        {
            _areaService = areaService;
            _locationService = locationService;
            _userService = userService;
        }

        /// <summary>
        /// Get a Area by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var res =  await _areaService.GetByIdAsync(id);
            return Ok(res);
        }

        /// <summary>
        /// Creates a Area.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateAreaRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _areaService.CreateAsync(request);

            return CreatedAtAction(null, res);
        }

        /// <summary>
        /// Deletes a Area.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var res = await _areaService.DeleteAsync(id);
            return res ? NoContent(): NotFound(new ProblemDetails() { Title = $"Area with id '{id} not found.'"});
        }

        /// <summary>
        /// Updates a Area.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [WorkspaceAuthorize("Area")]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromQuery] string id, [FromBody] UpdateAreaRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            if (request.NewDescription == null && request.NewName == null) return BadRequest(new ProblemDetails() { Title = "At least one field is required."});

            var res =  await _areaService.UpdateAsync(id, request);
            return Ok(res);
        }

        /// <summary>
        /// Get areas by location id.
        /// </summary>
        [HttpGet("Location")]
        [Authorize]
        [WorkspaceAuthorize("Area")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAreasByLocationId([FromQuery] string id)
        {   
            var areas = await _areaService.GetAreasByLocationIdAsync(id);
            return Ok(areas);
        }
    }
}
