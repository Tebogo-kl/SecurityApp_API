using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var userId = Guid.Parse(User.Identity?.Name ?? throw new UnauthorizedAccessException());
            var result = await _profileService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDto>> GetById(Guid id)
        {
            var result = await _profileService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
