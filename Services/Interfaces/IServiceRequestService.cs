using Secu.DataTransferObjects;
using SecurityService.API.Models;

namespace Secu.Services.Interfaces
{
    public interface IServiceRequestService
    {
        Task<ServiceRequestDto> CreateAsync(CreateServiceRequestDto dto, Guid userId);
        Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(Guid userId);
        Task<ServiceRequestDto?> UpdateStatusAsync(Guid id, RequestStatus status);
    }
}
