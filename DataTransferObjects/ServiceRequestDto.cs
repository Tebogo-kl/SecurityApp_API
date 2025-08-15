namespace Secu.DataTransferObjects
{
    public class ServiceRequestDto
    {
        public Guid Id { get; set; }
        public ServiceType ServiceType { get; set; }
        public string LocationAddress { get; set; } = string.Empty;
        public RequestStatus Status { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
