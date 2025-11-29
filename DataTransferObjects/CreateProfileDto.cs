using SecurityService.API.Models;
using System.Text.Json.Serialization;

namespace Secu.DataTransferObjects
{
    public class CreateProfileDto
    {
        [JsonPropertyName("Email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("Password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("address")]  
        public string Address { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
