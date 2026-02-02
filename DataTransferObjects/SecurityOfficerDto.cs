namespace Secu.DataTransferObjects
{
    public class SecurityOfficerDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string BadgeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<string>? Specializations { get; set; }
        public DateTime CreatedAt { get; set; }

        // PSIRA fields
        public string? PsiraNumber { get; set; }
        public bool IsPsiraVerified { get; set; }
        public string? PsiraStatus { get; set; }
        public DateTime? PsiraCertificateExpiry { get; set; }

        // Location fields - for showing officers on map
        public decimal? CurrentLatitude { get; set; }
        public decimal? CurrentLongitude { get; set; }

        // Stats
        public decimal Rating { get; set; }
        public int TotalJobs { get; set; }

        // Company info for display
        public string CompanyName { get; set; } = string.Empty;
    }

    public class CreateSecurityOfficerDto
    {
        public Guid CompanyId { get; set; }
        public string BadgeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = "Offline";
        public List<string>? Specializations { get; set; }
        public string? PsiraNumber { get; set; }
        public bool IsPsiraVerified { get; set; }
        public string? PsiraStatus { get; set; }
        public DateTime? PsiraCertificateExpiry { get; set; }
    }

    public class UpdateOfficerPsiraDto
    {
        public string PsiraNumber { get; set; } = string.Empty;
        public bool IsPsiraVerified { get; set; }
        public string? PsiraStatus { get; set; }
        public DateTime? PsiraCertificateExpiry { get; set; }
    }

    public class UpdateOfficerLocationDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}