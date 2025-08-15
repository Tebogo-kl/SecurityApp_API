using SecurityService.API.Models;

namespace Secu.DataTransferObjects
{
    public class CreateServiceRequestDto
    {
        public ServiceType ServiceType { get; set; }
        public decimal LocationLatitude { get; set; }
        public decimal LocationLongitude { get; set; }
        public string LocationAddress { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int EstimatedDuration { get; set; }
    }
}
