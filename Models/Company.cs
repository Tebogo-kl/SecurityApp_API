using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;

namespace SecurityService.API.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContactTitle { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? PsiraNumber { get; set; }

        public bool IsPsiraVerified { get; set; } = false;

        [StringLength(50)]
        public string? PsiraStatus { get; set; } // Registered, Withdrawn, Active, etc.

        public DateTime? PsiraCertificateExpiry { get; set; }

        public DateTime? PsiraVerifiedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<SecurityOfficer> SecurityOfficers { get; set; } = new List<SecurityOfficer>();
        public virtual Profile? Owner { get; set; }
    }
}
