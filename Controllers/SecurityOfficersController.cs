using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;

namespace Secu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityOfficersController : ControllerBase
    {
        private readonly ISecurityOfficerService _officerService;

        public SecurityOfficersController(ISecurityOfficerService officerService)
        {
            _officerService = officerService;
        }

        /// <summary>
        /// Create a new security officer
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateSecurityOfficerDto dto)
        {
            try
            {
                var result = await _officerService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create officer", error = ex.Message });
            }
        }

        /// <summary>
        /// Get all security officers
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _officerService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get all AVAILABLE security officers - This is what users see on the map
        /// </summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _officerService.GetAvailableAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get a specific security officer by their ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _officerService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Get security officer by user ID (profile ID)
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _officerService.GetByUserIdAsync(userId);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Get all security officers for a specific company
        /// </summary>
        [HttpGet("company/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetByCompanyId(Guid companyId)
        {
            var result = await _officerService.GetByCompanyIdAsync(companyId);
            return Ok(result);
        }

        /// <summary>
        /// Delete a security officer
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _officerService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Update officer status (Available, Busy, Offline)
        /// Called when officer goes online/offline or accepts a job
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            var result = await _officerService.UpdateStatusAsync(id, dto.Status);
            return result ? Ok(new { message = "Status updated successfully" }) : NotFound();
        }

        /// <summary>
        /// Update officer's GPS location
        /// Called periodically when officer is available to show on map
        /// </summary>
        [HttpPut("{id}/location")]
        public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] UpdateOfficerLocationDto dto)
        {
            var result = await _officerService.UpdateLocationAsync(id, dto.Latitude, dto.Longitude);
            return result ? Ok(new { message = "Location updated successfully" }) : NotFound();
        }

        /// <summary>
        /// Update PSIRA verification details
        /// </summary>
        [HttpPut("{id}/psira-verification")]
        public async Task<IActionResult> UpdatePsiraVerification(Guid id, [FromBody] UpdateOfficerPsiraDto dto)
        {
            try
            {
                var result = await _officerService.UpdatePsiraVerificationAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Officer not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update PSIRA verification", error = ex.Message });
            }
        }

        // DTO classes
        public class UpdateStatusDto
        {
            public string Status { get; set; } = string.Empty;
        }
    }
}