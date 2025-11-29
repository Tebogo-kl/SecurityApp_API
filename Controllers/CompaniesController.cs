using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;

namespace Secu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IProfileService _profileService;

        public CompaniesController(
            ICompanyService companyService,
            IProfileService profileService)
        {
            _companyService = companyService;
            _profileService = profileService;
        }

        // NEW: Public company registration endpoint
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegistrationDto dto)
        {
            try
            {
                // Validate password
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                {
                    return BadRequest(new { message = "Password must be at least 6 characters long" });
                }

                // Split contact person name
                var nameParts = dto.ContactPerson.Split(' ', 2);
                var firstName = nameParts[0];
                var lastName = nameParts.Length > 1 ? nameParts[1] : "";

                // Create profile for company admin (Role = CompanyAdmin)
                var profileDto = new CreateProfileDto
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    Role = UserRole.CompanyAdmin // Use the enum
                };

                var createdProfile = await _profileService.CreateAsync(profileDto);

                // Create company linked to the profile
                var companyDto = new CreateCompanyDto
                {
                    CompanyName = dto.CompanyName,
                    RegistrationNumber = dto.RegistrationNumber,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    ContactPerson = dto.ContactPerson,
                    ContactTitle = dto.ContactTitle
                };

                var createdCompany = await _companyService.CreateAsync(companyDto, createdProfile.Id);

                return Ok(new
                {
                    message = "Company registered successfully",
                    company = createdCompany,
                    profileId = createdProfile.Id
                });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Email already exists"))
            {
                return BadRequest(new { message = "Email already registered" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Registration failed",
                    error = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        // EXISTING: Authenticated endpoint to create company
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CompanyDto>> Create(CreateCompanyDto dto)
        {
            var ownerId = Guid.Parse(User.Identity?.Name ?? throw new UnauthorizedAccessException());
            var result = await _companyService.CreateAsync(dto, ownerId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(Guid id)
        {
            var result = await _companyService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var result = await _companyService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id}/psira-verification")]
        public async Task<IActionResult> UpdatePsiraVerification(Guid id, [FromBody] UpdateCompanyPsiraDto dto)
        {
            try
            {
                var result = await _companyService.UpdatePsiraVerificationAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update PSIRA verification", error = ex.Message });
            }
        }
    }
}