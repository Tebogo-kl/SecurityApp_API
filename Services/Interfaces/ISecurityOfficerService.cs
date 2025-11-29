using Secu.DataTransferObjects;




namespace Secu.Services.Interfaces
{
    public interface ISecurityOfficerService
    {
        Task<SecurityOfficerDto> CreateAsync(CreateSecurityOfficerDto dto);
        Task<SecurityOfficerDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<SecurityOfficerDto>> GetByCompanyIdAsync(Guid companyId);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, string status);
        Task<SecurityOfficerDto?> GetByUserIdAsync(Guid userId);
        Task<SecurityOfficerDto> UpdatePsiraVerificationAsync(Guid id, UpdateOfficerPsiraDto dto);
    }
}
