using Secu.DataTransferObjects;
using SecurityService.API.Models;

namespace Secu.Services.Interfaces
{
    public interface IServiceRequestService
    {
        Task<ServiceRequestDto> CreateAsync(CreateServiceRequestDto dto, Guid userId);
        Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(Guid userId);
        Task<IEnumerable<ServiceRequestDto>> GetPendingAsync();
        Task<IEnumerable<ServiceRequestDto>> GetByOfficerAsync(Guid officerId);
        Task<ServiceRequestDto?> GetByIdAsync(Guid id);
        Task<ServiceRequestDto?> UpdateStatusAsync(Guid id, RequestStatus status);
        Task<ServiceRequestDto?> AcceptRequestAsync(Guid requestId, Guid officerId);
        Task<ServiceRequestDto?> CompleteRequestAsync(Guid requestId, decimal? totalCost);
        Task<ServiceRequestDto?> CancelRequestAsync(Guid requestId);
    }
}