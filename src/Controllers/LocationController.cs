using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
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
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Get a location by id.
        /// </summary>
        [HttpGet("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LocationDto>> GetById([FromQuery] string id)
        {
            return await _locationService.GetByIdAsync(id);
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

            return await _locationService.CreateAsync(request.WorkspaceId, request.Name, request.Description, request.Address, request.City, request.Country);
        }

        /// <summary>
        /// Deletes a location.
        /// </summary>
        [HttpDelete("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            return await _locationService.DeleteAsync(id);
        }

        /// <summary>
        /// Updates a location.
        /// </summary>
        [HttpPut("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK / StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationDto>> Update([FromQuery] string id, [FromBody] UpdateLocationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage).ToList());

            return await _locationService.UpdateAsync(id, request.NewName, request.NewDescription, request.NewAddress, request.NewCity, request.NewCountry);
        }
    }
}
