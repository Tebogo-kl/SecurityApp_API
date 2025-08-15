using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecurityService.API.Models
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid RequestId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid OfficerId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("RequestId")]
        public virtual ServiceRequest Request { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual Profile User { get; set; } = null!;

        [ForeignKey("OfficerId")]
        public virtual SecurityOfficer Officer { get; set; } = null!;
    }
}
