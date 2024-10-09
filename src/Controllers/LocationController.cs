using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IWorkspaceService _workspaceService;
        public LocationController(ILocationService locationService, IWorkspaceService workspaceService)
        {
            _locationService = locationService;
            _workspaceService = workspaceService;
        }

        /// <summary>
        /// Get a location by id.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto?>> GetById(string id)
        {
            var res = await _locationService.GetByIdAsync(id);
            if (!res.Success) return NotFound(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Creates a location.
        /// </summary>
        [HttpPost("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationDto>> Create([FromBody] CreateLocationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _locationService.CreateAsync(request.WorkspaceId, request.Name, request.Description, request.Address, request.City, request.Country);
            if (!res.Success) return BadRequest(res.Errors);

            return CreatedAtAction(nameof(Create), res.Content);
        }

        /// <summary>
        /// Deletes a location.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _locationService.DeleteAsync(id);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }

        /// <summary>
        /// Updates a location.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateLocationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            var res = await _locationService.UpdateAsync(id, request.NewName, request.NewDescription, request.NewAddress, request.NewCity, request.NewCountry);
            if (!res.Success) return BadRequest(res.Errors);

            return Ok(res.Content);
        }
    }
}
