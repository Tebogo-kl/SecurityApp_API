using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecurityService.API.Models
{
    public class ServiceRequest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public Guid? OfficerId { get; set; } // Nullable - assigned later

        [Required]
        public ServiceType ServiceType { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,8)")]
        public decimal LocationLatitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(11,8)")]
        public decimal LocationLongitude { get; set; }

        [Required]
        [StringLength(500)]
        public string LocationAddress { get; set; } = string.Empty;

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public int EstimatedDuration { get; set; } // in minutes

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual Profile User { get; set; } = null!;

        [ForeignKey("OfficerId")]
        public virtual SecurityOfficer? AssignedOfficer { get; set; }

        //public virtual ICollection<RequestResponse> Responses { get; set; } = new List<RequestResponse>();
        //public virtual Review? Review { get; set; }
    }
}

