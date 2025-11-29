namespace Secu.DataTransferObjects
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? PsiraNumber { get; set; }
        public bool IsPsiraVerified { get; set; }
        public string? PsiraStatus { get; set; }
        public DateTime? PsiraCertificateExpiry { get; set; }
    }

    public class UpdateCompanyPsiraDto
    {
        public string PsiraNumber { get; set; } = string.Empty;
        public bool IsPsiraVerified { get; set; }
        public string? PsiraStatus { get; set; }
        public DateTime? PsiraCertificateExpiry { get; set; }
    }
}
