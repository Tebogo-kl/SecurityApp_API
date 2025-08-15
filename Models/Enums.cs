namespace SecurityService.API.Models
{
    // Enums
    public enum UserRole
    {
        User,
        CompanyAdmin,
        SecurityOfficer
    }

    public enum ServiceType
    {
        ArmedResponse,
        Patrol,
        Escort,
        Emergency,
        Surveillance
    }

    public enum RequestStatus
    {
        Pending,
        Accepted,
        InProgress,
        Completed,
        Cancelled
    }

    public enum OfficerStatus
    {
        Available,
        Busy,
        Offline
    }
}