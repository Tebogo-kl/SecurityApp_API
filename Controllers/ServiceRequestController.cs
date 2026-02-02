using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;
using System.Security.Claims;

namespace Secu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequestService;

        public ServiceRequestsController(IServiceRequestService serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }

        /// <summary>
        /// Create a new service request (User action)
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ServiceRequestDto>> Create([FromBody] CreateServiceRequestDto dto)
        {
            try
            {
                // Get user ID from JWT token
                var userIdClaim = User.FindFirst("userId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Unauthorized(new { message = "Invalid user token" });
                }

                var result = await _serviceRequestService.CreateAsync(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create request", error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific request by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequestDto>> GetById(Guid id)
        {
            var result = await _serviceRequestService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Get all requests for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetByUser(Guid userId)
        {
            var result = await _serviceRequestService.GetByUserAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get current user's requests
        /// </summary>
        [HttpGet("my-requests")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetMyRequests()
        {
            var userIdClaim = User.FindFirst("userId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var result = await _serviceRequestService.GetByUserAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get all pending requests (For officers to see available jobs)
        /// </summary>
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetPending()
        {
            var result = await _serviceRequestService.GetPendingAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get all requests assigned to a specific officer
        /// </summary>
        [HttpGet("officer/{officerId}")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetByOfficer(Guid officerId)
        {
            var result = await _serviceRequestService.GetByOfficerAsync(officerId);
            return Ok(result);
        }

        /// <summary>
        /// Update request status
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<ActionResult<ServiceRequestDto>> UpdateStatus(Guid id, [FromBody] UpdateStatusRequestDto dto)
        {
            var result = await _serviceRequestService.UpdateStatusAsync(id, dto.Status);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Officer accepts a pending request
        /// </summary>
        [HttpPut("{id}/accept")]
        public async Task<ActionResult<ServiceRequestDto>> AcceptRequest(Guid id, [FromBody] AcceptRequestDto dto)
        {
            try
            {
                var result = await _serviceRequestService.AcceptRequestAsync(id, dto.OfficerId);
                return result == null ? NotFound() : Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to accept request", error = ex.Message });
            }
        }

        /// <summary>
        /// Officer completes a request
        /// </summary>
        [HttpPut("{id}/complete")]
        public async Task<ActionResult<ServiceRequestDto>> CompleteRequest(Guid id, [FromBody] CompleteRequestDto dto)
        {
            try
            {
                var result = await _serviceRequestService.CompleteRequestAsync(id, dto.TotalCost);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to complete request", error = ex.Message });
            }
        }

        /// <summary>
        /// Cancel a request
        /// </summary>
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<ServiceRequestDto>> CancelRequest(Guid id)
        {
            try
            {
                var result = await _serviceRequestService.CancelRequestAsync(id);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to cancel request", error = ex.Message });
            }
        }
    }
}