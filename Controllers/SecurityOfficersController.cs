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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _officerService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("company/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetByCompanyId(Guid companyId)
        {
            var result = await _officerService.GetByCompanyIdAsync(companyId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _officerService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("{id}/status")]

        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            var result = await _officerService.UpdateStatusAsync(id, dto.Status);

            return result ? Ok() : NotFound();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _officerService.GetByUserIdAsync(userId);
            return result == null ? NotFound() : Ok(result);
        }
        public class UpdateStatusDto
        {
            public string Status { get; set; } = string.Empty;
        }
    

   

    [HttpPut("{id}/psira-verification")]
        public async Task<IActionResult> UpdatePsiraVerification(Guid id, [FromBody] UpdateOfficerPsiraDto dto)
        {
            try
            {
                var result = await _officerService.UpdatePsiraVerificationAsync(id, dto);
                return Ok(result);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update PSIRA verification", error = ex.Message });
            }
        }
    }
}
