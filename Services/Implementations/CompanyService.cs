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
            CompanyName = company.CompanyName,
            Email = company.Email,
            Phone = company.Phone
        };
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        var company = await _context.Companies.FindAsync(id);
        return company == null ? null : new CompanyDto
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            Email = company.Email,
            Phone = company.Phone
        };
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        return await _context.Companies
            .Select(c => new CompanyDto
            {
                Id = c.Id,
                CompanyName = c.CompanyName,
                Email = c.Email,
                Phone = c.Phone
            })
            .ToListAsync();
    }
}
    
