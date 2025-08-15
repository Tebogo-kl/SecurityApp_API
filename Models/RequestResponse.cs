using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecurityService.API.Models
{
    public class RequestResponse
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid RequestId { get; set; }

        [Required]
        public Guid OfficerId { get; set; }

        public TimeSpan ResponseTime { get; set; } // Time taken to respond

        public DateTime EstimatedArrival { get; set; }

        [StringLength(500)]
        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("RequestId")]
        public virtual ServiceRequest Request { get; set; } = null!;

        [ForeignKey("OfficerId")]
        public virtual SecurityOfficer Officer { get; set; } = null!;
    }
}

