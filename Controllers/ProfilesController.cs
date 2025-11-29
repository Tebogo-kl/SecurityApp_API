using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;

namespace Secu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        public async Task<ActionResult<ProfileDto>> Create(CreateProfileDto dto)
        {
            //var userId = Guid.Parse(User.Identity?.Name ?? throw new UnauthorizedAccessException());
            var userId = Guid.NewGuid();
            var result = await _profileService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
        {
            try
            {
                var result = await _profileService.LoginAsync(dto);

                if (result == null)
                {
                    return Unauthorized(new { message = "Invalid Login Details" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Login failed", error = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDto>> GetById(Guid id)
        {
            var result = await _profileService.GetByIdAsync(id); 
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("officer-login")]
        public async Task<ActionResult<LoginResponseDto>> OfficerLogin([FromBody] OfficerLoginDto dto)
        {
            try
            {
                var result = await _profileService.OfficerLoginAsync(dto);

                if (result == null)
                {
                    return Unauthorized(new { message = "Invalid badge number or name" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Officer login failed", error = ex.Message });
            }
        }
    }
}
