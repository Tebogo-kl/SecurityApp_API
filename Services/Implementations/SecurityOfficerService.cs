using Microsoft.EntityFrameworkCore;
using Secu.Data;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;

namespace Secu.Services.Implementations
{
    public class SecurityOfficerService : ISecurityOfficerService
    {
        private readonly SecurityServiceDbContext _context;

        public SecurityOfficerService(SecurityServiceDbContext context)
        {
            _context = context;
        }

        public async Task<SecurityOfficerDto> CreateAsync(CreateSecurityOfficerDto dto)
        {
            // Parse the status string to enum
            if (!Enum.TryParse<OfficerStatus>(dto.Status, true, out var statusEnum))
            {
                statusEnum = OfficerStatus.Offline; // Default to offline if parsing fails
            }

            // Create a profile for the security officer
            var profile = new Profile
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("DefaultPassword123!"), // Temporary password
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Address = "Not specified",
                Role = UserRole.SecurityOfficer,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Profiles.Add(profile);

            // Create security officer
            var officer = new SecurityOfficer
            {
                Id = Guid.NewGuid(),
                UserId = profile.Id,
                CompanyId = dto.CompanyId,
                BadgeNumber = dto.BadgeNumber,
                Status = statusEnum,  // Use the enum
                Specializations = dto.Specializations,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SecurityOfficers.Add(officer);
            await _context.SaveChangesAsync();

            return new SecurityOfficerDto
            {
                Id = officer.Id,
                CompanyId = officer.CompanyId,
                BadgeNumber = officer.BadgeNumber,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Email = profile.Email,
                Status = officer.Status.ToString(),  // Convert enum to string
                Specializations = officer.Specializations,
                CreatedAt = officer.CreatedAt
            };
        }

        public async Task<SecurityOfficerDto?> GetByIdAsync(Guid id)
        {
            var officer = await _context.SecurityOfficers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (officer == null) return null;

            return new SecurityOfficerDto
            {
                Id = officer.Id,
                CompanyId = officer.CompanyId,
                BadgeNumber = officer.BadgeNumber,
                FirstName = officer.User?.FirstName ?? "",
                LastName = officer.User?.LastName ?? "",
                Phone = officer.User?.Phone ?? "",
                Email = officer.User?.Email ?? "",
                Status = officer.Status.ToString(),  // Convert enum to string
                Specializations = officer.Specializations,
                CreatedAt = officer.CreatedAt
            };
        }


        public async Task<SecurityOfficerDto?> GetByUserIdAsync(Guid userId)
        {
            var officer = await _context.SecurityOfficers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (officer == null) return null;

            return new SecurityOfficerDto
            {
                Id = officer.Id,
                CompanyId = officer.CompanyId,
                BadgeNumber = officer.BadgeNumber,
                FirstName = officer.User?.FirstName ?? "",
                LastName = officer.User?.LastName ?? "",
                Phone = officer.User?.Phone ?? "",
                Email = officer.User?.Email ?? "",
                Status = officer.Status.ToString(),
                Specializations = officer.Specializations,
                CreatedAt = officer.CreatedAt
            };
        }
        public async Task<IEnumerable<SecurityOfficerDto>> GetByCompanyIdAsync(Guid companyId)
        {
            var officers = await _context.SecurityOfficers
                .Include(o => o.User)
                .Where(o => o.CompanyId == companyId)
                .ToListAsync();

            return officers.Select(officer => new SecurityOfficerDto
            {
                Id = officer.Id,
                CompanyId = officer.CompanyId,
                BadgeNumber = officer.BadgeNumber,
                FirstName = officer.User?.FirstName ?? "",
                LastName = officer.User?.LastName ?? "",
                Phone = officer.User?.Phone ?? "",
                Email = officer.User?.Email ?? "",
                Status = officer.Status.ToString(),  // Convert enum to string
                Specializations = officer.Specializations,
                CreatedAt = officer.CreatedAt
            });
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var officer = await _context.SecurityOfficers.FindAsync(id);
            if (officer == null) return false;

            _context.SecurityOfficers.Remove(officer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var officer = await _context.SecurityOfficers.FindAsync(id);
            if (officer == null) return false;

            // Parse string to enum
            if (Enum.TryParse<OfficerStatus>(status, true, out var statusEnum))
            {
                officer.Status = statusEnum;
                officer.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<SecurityOfficerDto> UpdatePsiraVerificationAsync(Guid officerId, UpdateOfficerPsiraDto dto)
        {
            var officer = await _context.SecurityOfficers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == officerId);

            if (officer == null) throw new KeyNotFoundException("Officer not found");

            officer.PsiraNumber = dto.PsiraNumber;
            officer.IsPsiraVerified = dto.IsPsiraVerified;
            officer.PsiraStatus = dto.PsiraStatus;
            officer.PsiraCertificateExpiry = dto.PsiraCertificateExpiry;
            officer.PsiraVerifiedAt = DateTime.UtcNow;
            officer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new SecurityOfficerDto
            {
                Id = officer.Id,
                CompanyId = officer.CompanyId,
                BadgeNumber = officer.BadgeNumber,
                FirstName = officer.User?.FirstName ?? "",
                LastName = officer.User?.LastName ?? "",
                Phone = officer.User?.Phone ?? "",
                Email = officer.User?.Email ?? "",
                Status = officer.Status.ToString(),
                Specializations = officer.Specializations,
                PsiraNumber = officer.PsiraNumber,
                IsPsiraVerified = officer.IsPsiraVerified,
                PsiraStatus = officer.PsiraStatus,
                PsiraCertificateExpiry = officer.PsiraCertificateExpiry,
                CreatedAt = officer.CreatedAt
            };
        }
    }
}