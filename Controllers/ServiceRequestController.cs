using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;

namespace Secu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestService _serviceRequestService;

        public ServiceRequestController(IServiceRequestService serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceRequestDto>> Create(CreateServiceRequestDto dto)
        {
            var userId = Guid.Parse(User.Identity?.Name ?? throw new UnauthorizedAccessException());
            var result = await _serviceRequestService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetByUser), new { userId = result.Id }, result);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetByUser(Guid userId)
        {
            var result = await _serviceRequestService.GetByUserAsync(userId);
            return Ok(result);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<ServiceRequestDto>> UpdateStatus(Guid id, [FromBody] RequestStatus status)
        {
            var result = await _serviceRequestService.UpdateStatusAsync(id, status);
            return result == null ? NotFound() : Ok(result);
        }
    }
}

