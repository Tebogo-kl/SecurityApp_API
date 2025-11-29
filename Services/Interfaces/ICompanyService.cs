using Secu.DataTransferObjects;

namespace Secu.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> CreateAsync(CreateCompanyDto dto, Guid ownerId);
        Task<CompanyDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<CompanyDto>> GetAllAsync();
        Task<CompanyDto> UpdatePsiraVerificationAsync(Guid companyId, UpdateCompanyPsiraDto dto); // ADD THIS
    }
}
