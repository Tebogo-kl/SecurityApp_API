using Microsoft.EntityFrameworkCore;
using Secu.Data;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;
using SecurityService.API.Models;

public class CompanyService : ICompanyService
{
    private readonly SecurityServiceDbContext _context;

    public CompanyService(SecurityServiceDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto, Guid ownerId)
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            CompanyName = dto.CompanyName,
            RegistrationNumber = dto.RegistrationNumber,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            ContactPerson = dto.ContactPerson,
            ContactTitle = dto.ContactTitle
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return new CompanyDto
        {
            Id = company.Id,
            OwnerId = company.OwnerId,
            CompanyName = company.CompanyName,
            Email = company.Email,
            Phone = company.Phone
        };
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null) return null;

        return new CompanyDto
        {
            Id = company.Id,
            OwnerId = company.OwnerId,
            CompanyName = company.CompanyName,
            Email = company.Email,
            Phone = company.Phone
        };
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var companies = await _context.Companies.ToListAsync();

        return companies.Select(c => new CompanyDto
        {
            Id = c.Id,
            OwnerId = c.OwnerId,
            CompanyName = c.CompanyName,
            Email = c.Email, 
            Phone = c.Phone
        });
    }


    public async Task<CompanyDto> UpdatePsiraVerificationAsync(Guid companyId, UpdateCompanyPsiraDto dto)
    {
        var company = await _context.Companies.FindAsync(companyId);
        if (company == null) throw new KeyNotFoundException("Company not found");

        company.PsiraNumber = dto.PsiraNumber;
        company.IsPsiraVerified = dto.IsPsiraVerified;
        company.PsiraStatus = dto.PsiraStatus;
        company.PsiraCertificateExpiry = dto.PsiraCertificateExpiry;
        company.PsiraVerifiedAt = DateTime.UtcNow;
        company.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new CompanyDto
        {
            Id = company.Id,
            OwnerId = company.OwnerId,
            CompanyName = company.CompanyName,
            Email = company.Email,
            Phone = company.Phone,
            PsiraNumber = company.PsiraNumber,
            IsPsiraVerified = company.IsPsiraVerified,
            PsiraStatus = company.PsiraStatus,
            PsiraCertificateExpiry = company.PsiraCertificateExpiry
        };
    }
}