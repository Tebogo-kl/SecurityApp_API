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

        public async Task<ServiceRequestDto>  CreateAsync(CreateServiceRequestDto dto, Guid userId)
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
                EstimatedDuration = dto.EstimatedDuration,
                Status = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            // Fetch with user profile for response
            var created = await _context.ServiceRequests
                .Include(sr => sr.User)
                .FirstOrDefaultAsync(sr => sr.Id == request.Id);

            return MapToDto(created!);
        }

        public async Task<ServiceRequestDto?> GetByIdAsync(Guid id)
        {
            var request = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Include(sr => sr.AssignedOfficer)
                    .ThenInclude(o => o!.User)
                .FirstOrDefaultAsync(sr => sr.Id == id);

            return request == null ? null : MapToDto(request);
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetByUserAsync(Guid userId)
        {
            var requests = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Include(sr => sr.AssignedOfficer)
                    .ThenInclude(o => o!.User)
                .Where(sr => sr.UserId == userId)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();

            return requests.Select(MapToDto);
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetPendingAsync()
        {
            var requests = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Where(sr => sr.Status == RequestStatus.Pending)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();

            return requests.Select(MapToDto);
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetByOfficerAsync(Guid officerId)
        {
            var requests = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Include(sr => sr.AssignedOfficer)
                    .ThenInclude(o => o!.User)
                .Where(sr => sr.OfficerId == officerId)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();

            return requests.Select(MapToDto);
        }

        public async Task<ServiceRequestDto?> UpdateStatusAsync(Guid id, RequestStatus status)
        {
            var request = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Include(sr => sr.AssignedOfficer)
                    .ThenInclude(o => o!.User)
                .FirstOrDefaultAsync(sr => sr.Id == id);

            if (request == null) return null;

            request.Status = status;
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(request);
        }

        public async Task<ServiceRequestDto?> AcceptRequestAsync(Guid requestId, Guid officerId)
        {
            var request = await _context.ServiceRequests
                .Include(sr => sr.User)
                .FirstOrDefaultAsync(sr => sr.Id == requestId);

            if (request == null) return null;

            // Check if request is still pending
            if (request.Status != RequestStatus.Pending)
            {
                throw new InvalidOperationException("Request is no longer available");
            }

            // Get the officer
            var officer = await _context.SecurityOfficers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == officerId);

            if (officer == null)
            {
                throw new KeyNotFoundException("Officer not found");
            }

            // Update request
            request.OfficerId = officerId;
            request.Status = RequestStatus.Accepted;
            request.UpdatedAt = DateTime.UtcNow;

            // Update officer status to busy
            officer.Status = OfficerStatus.Busy;
            officer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload with includes
            request = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Include(sr => sr.AssignedOfficer)
                    .ThenInclude(o => o!.User)
                .FirstOrDefaultAsync(sr => sr.Id == requestId);

            return MapToDto(request!);
        }

        public async Task<ServiceRequestDto?> CompleteRequestAsync(Guid requestId, decimal? totalCost)
        {
            var request = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Include(sr => sr.AssignedOfficer)
                    .ThenInclude(o => o!.User)
                .FirstOrDefaultAsync(sr => sr.Id == requestId);

            if (request == null) return null;

            request.Status = RequestStatus.Completed;
            request.TotalCost = totalCost;
            request.UpdatedAt = DateTime.UtcNow;

            // Set officer back to available
            if (request.AssignedOfficer != null)
            {
                request.AssignedOfficer.Status = OfficerStatus.Available;
                request.AssignedOfficer.TotalJobs += 1;
                request.AssignedOfficer.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return MapToDto(request);
        }

        public async Task<ServiceRequestDto?> CancelRequestAsync(Guid requestId)
        {
            var request = await _context.ServiceRequests
                .Include(sr => sr.User)
                .Include(sr => sr.AssignedOfficer)
                    .ThenInclude(o => o!.User)
                .FirstOrDefaultAsync(sr => sr.Id == requestId);

            if (request == null) return null;

            request.Status = RequestStatus.Cancelled;
            request.UpdatedAt = DateTime.UtcNow;

            // Set officer back to available if assigned
            if (request.AssignedOfficer != null)
            {
                request.AssignedOfficer.Status = OfficerStatus.Available;
                request.AssignedOfficer.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return MapToDto(request);
        }

        // Helper method to map entity to DTO
        private static ServiceRequestDto MapToDto(ServiceRequest request)
        {
            return new ServiceRequestDto
            {
                Id = request.Id,
                UserId = request.UserId,
                OfficerId = request.OfficerId,
                ServiceType = request.ServiceType,
                LocationLatitude = request.LocationLatitude,
                LocationLongitude = request.LocationLongitude,
                LocationAddress = request.LocationAddress,
                Description = request.Description,
                Status = request.Status,
                EstimatedDuration = request.EstimatedDuration,
                TotalCost = request.TotalCost,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                // User profile info
                UserProfile = request.User != null ? new UserProfileDto
                {
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Phone = request.User.Phone,
                    Email = request.User.Email
                } : null,
                // Officer info
                OfficerInfo = request.AssignedOfficer != null ? new OfficerInfoDto
                {
                    Id = request.AssignedOfficer.Id,
                    FirstName = request.AssignedOfficer.User?.FirstName ?? "",
                    LastName = request.AssignedOfficer.User?.LastName ?? "",
                    Phone = request.AssignedOfficer.User?.Phone ?? "",
                    BadgeNumber = request.AssignedOfficer.BadgeNumber,
                    Status = request.AssignedOfficer.Status.ToString()
                } : null
            };
        }
    }
}