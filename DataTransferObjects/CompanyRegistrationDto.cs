namespace Secu.DataTransferObjects
{
    public class CompanyRegistrationDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactTitle { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
