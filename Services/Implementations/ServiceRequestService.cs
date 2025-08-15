using Microsoft.EntityFrameworkCore;
using Secu.Data;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;

namespace Secu.Services.Implementations
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly SecurityServiceDbContext _context;

        public ServiceRequestService(SecurityServiceDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceRequestDto> CreateAsync(CreateServiceRequestDto dto, Guid userId)
        {
            var request = new ServiceRequest
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ServiceType = dto.ServiceType,
                LocationLatitude = dto.LocationLatitude,
                LocationLongitude = dto.LocationLongitude,
                LocationAddress = dto.LocationAddress,
                Description = dto.Description,
                EstimatedDuration = dto.EstimatedDuration
            };

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return new ServiceRequestDto
            {
                Id = request.Id,
                ServiceType = request.ServiceType,
                LocationAddress = request.LocationAddress,
                Status = request.Status,
                TotalCost = request.TotalCost,
                CreatedAt = request.CreatedAt
            };
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(Guid userId)
        {
            return await _context.ServiceRequests
                .Where(sr => sr.UserId == userId)
                .Select(sr => new ServiceRequestDto
                {
                    Id = sr.Id,
                    ServiceType = sr.ServiceType,
                    LocationAddress = sr.LocationAddress,
                    Status = sr.Status,
                    TotalCost = sr.TotalCost,
                    CreatedAt = sr.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ServiceRequestDto?> UpdateStatusAsync(Guid id, RequestStatus status)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return null;

            request.Status = status;
            request.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new ServiceRequestDto
            {
                Id = request.Id,
                ServiceType = request.ServiceType,
                LocationAddress = request.LocationAddress,
                Status = request.Status,
                TotalCost = request.TotalCost,
                CreatedAt = request.CreatedAt
            };
        }
    }
}
