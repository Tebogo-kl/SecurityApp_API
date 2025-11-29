using SecurityService.API.Models;

namespace Secu.DataTransferObjects
{
    public class ProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    } 


    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string BadgeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public ProfileDto Profiles { get; set; } = null!;
        public int ExpiresIn { get; set; } = 3600;
    }

    public class OfficerLoginDto
    {
        public string FullName { get; set; } = string.Empty;
        public string BadgeNumber { get; set; } = string.Empty;
    }


}
