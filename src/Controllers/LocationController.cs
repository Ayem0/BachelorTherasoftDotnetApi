using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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
            var location = await _locationService.GetByIdAsync(id);
            if (location == null) return NotFound();
  
            return Ok(location);
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
            
            var location = await _locationService.CreateAsync(request.WorkspaceId, request.Name, request.Description, request.Address, request.City, request.Country);
            if (location == null) return BadRequest();

            return CreatedAtAction(nameof(Create), new { id = location.Id }, location);
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
            if (!res) return BadRequest();

            return Ok();
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
            if (res == null) return BadRequest();

            return Ok();
        }
    }
}
