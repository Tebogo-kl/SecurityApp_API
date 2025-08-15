
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecurityService.API.Models
{
    public class SecurityOfficer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        public string BadgeNumber { get; set; } = string.Empty;

        [Required]
        public OfficerStatus Status { get; set; } = OfficerStatus.Offline;

        [Column(TypeName = "decimal(10,2)")]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(3,2)")]
        [Range(0, 5)]
        public decimal Rating { get; set; }

        public int TotalJobs { get; set; } = 0;

        [Column(TypeName = "decimal(10,8)")]
        public decimal? CurrentLatitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal? CurrentLongitude { get; set; }

        [StringLength(500)]
        public string? Specializations { get; set; } // JSON or comma-separated

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual Profile User { get; set; } = null!;

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; } = null!;

        public virtual ICollection<ServiceRequest> AssignedRequests { get; set; } = new List<ServiceRequest>();
        public virtual ICollection<RequestResponse> Responses { get; set; } = new List<RequestResponse>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

