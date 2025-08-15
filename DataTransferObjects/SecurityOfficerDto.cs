namespace Secu.DataTransferObjects
{
    public class SecurityOfficerDto
    {
        public Guid Id { get; set; }
        public string BadgeNumber { get; set; } = string.Empty;
        public OfficerStatus Status { get; set; }
        public decimal Rating { get; set; }
        public decimal? CurrentLatitude { get; set; }
        public decimal? CurrentLongitude { get; set; }
    }
}
