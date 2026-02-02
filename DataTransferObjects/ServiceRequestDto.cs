using SecurityService.API.Models;

namespace Secu.DataTransferObjects
{
    public class ServiceRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? OfficerId { get; set; }
        public ServiceType ServiceType { get; set; }
        public decimal LocationLatitude { get; set; }
        public decimal LocationLongitude { get; set; }
        public string LocationAddress { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public RequestStatus Status { get; set; }
        public int EstimatedDuration { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Related data for display
        public UserProfileDto? UserProfile { get; set; }
        public OfficerInfoDto? OfficerInfo { get; set; }
    }

    //public class CreateServiceRequestDto
    //{
    //    public ServiceType ServiceType { get; set; }
    //    public decimal LocationLatitude { get; set; }
    //    public decimal LocationLongitude { get; set; }
    //    public string LocationAddress { get; set; } = string.Empty;
    //    public string Description { get; set; } = string.Empty;
    //    public int EstimatedDuration { get; set; }
    //    public Guid? PreferredOfficerId { get; set; } // Optional: user can request specific officer
    //}

    public class UserProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class OfficerInfoDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string BadgeNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class AcceptRequestDto
    {
        public Guid OfficerId { get; set; }
    }

    public class CompleteRequestDto
    {
        public decimal? TotalCost { get; set; }
    }

    public class UpdateStatusRequestDto
    {
        public RequestStatus Status { get; set; }
    }
}