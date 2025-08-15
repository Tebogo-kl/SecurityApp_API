using Secu.Data;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;

namespace Secu.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly SecurityServiceDbContext _context;

        public ProfileService(SecurityServiceDbContext context)
        {
            _context = context;
        }

        public async Task<ProfileDto> CreateAsync(CreateProfileDto dto, Guid userId)
        {
            var profile = new Profile
            {
                Id = userId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Address = dto.Address,
                Role = dto.Role
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return new ProfileDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Role = profile.Role
            };
        }

        public async Task<ProfileDto?> GetByIdAsync(Guid id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            return profile == null ? null : new ProfileDto
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Role = profile.Role
            };
        }
    }
}
