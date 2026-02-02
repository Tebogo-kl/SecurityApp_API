using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Secu.Data;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Secu.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly SecurityServiceDbContext _context;
        private readonly IConfiguration _configuration;

        public ProfileService(SecurityServiceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ProfileDto> CreateAsync(CreateProfileDto dto)
        {
            return await CreateAsync(dto, Guid.NewGuid());
        }

        public async Task<ProfileDto> CreateAsync(CreateProfileDto dto, Guid userId)
        {
            var existing = await _context.Profiles
               .FirstOrDefaultAsync(p => p.Email.ToLower() == dto.Email.ToLower());

            if (existing != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var profile = new Profile
            {
                Id = userId,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
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
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Role = profile.Role
            };
        }

        public async Task<ProfileDto?> GetByIdAsync(Guid userId)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            return profile == null ? null : new ProfileDto
            {
                Id = profile.Id,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Role = profile.Role
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
                UserId = officer.UserId,
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

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.Email.ToLower() == dto.Email.ToLower());

            if (profile == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, profile.PasswordHash))
            {
                return null;
            }

            var token = GenerateJwtToken(profile);

            return new LoginResponseDto
            {
                Token = token,
                Profiles = new ProfileDto
                {
                    Id = profile.Id,
                    Email = profile.Email,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Phone = profile.Phone,
                    Role = profile.Role
                },
                ExpiresIn = 3600
            };
        }

        private string GenerateJwtToken(Profile profile)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "YourDefaultSecretKeyHere123456789!";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, profile.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, profile.Email),
                new Claim("userId", profile.Id.ToString()),
                new Claim("role", profile.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "SecurityServiceAPI",
                audience: "SecurityServiceClient",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResponseDto?> OfficerLoginAsync(OfficerLoginDto dto)
        {
            try
            {
                var officer = await _context.SecurityOfficers
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.BadgeNumber == dto.BadgeNumber);

                if (officer == null || officer.User == null)
                {
                    return null;
                }

                var fullName = $"{officer.User.FirstName} {officer.User.LastName}".Trim().ToLower();
                var providedName = dto.FullName.Trim().ToLower();

                if (!fullName.Contains(providedName) && !providedName.Contains(officer.User.FirstName.ToLower()))
                {
                    return null;
                }

                var token = GenerateJwtToken(officer.User);

                return new LoginResponseDto
                {
                    Token = token,
                    Profiles = new ProfileDto
                    {
                        Id = officer.User.Id,
                        Email = officer.User.Email,
                        FirstName = officer.User.FirstName,
                        LastName = officer.User.LastName,
                        Phone = officer.User.Phone,
                        Role = officer.User.Role
                    },
                    ExpiresIn = 3600
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Officer login error: {ex.Message}");
                return null;
            }
        }
    }
}