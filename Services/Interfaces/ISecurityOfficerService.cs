using Secu.DataTransferObjects;

namespace Secu.Services.Interfaces
{
    public interface ISecurityOfficerService
    {
        Task<SecurityOfficerDto> CreateAsync(CreateSecurityOfficerDto dto);
        Task<SecurityOfficerDto?> GetByIdAsync(Guid id);
        Task<SecurityOfficerDto?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<SecurityOfficerDto>> GetByCompanyIdAsync(Guid companyId);
        Task<IEnumerable<SecurityOfficerDto>> GetAllAsync();
        Task<IEnumerable<SecurityOfficerDto>> GetAvailableAsync();
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, string status);
        Task<bool> UpdateLocationAsync(Guid id, decimal latitude, decimal longitude);
        Task<SecurityOfficerDto> UpdatePsiraVerificationAsync(Guid id, UpdateOfficerPsiraDto dto);
    }
}