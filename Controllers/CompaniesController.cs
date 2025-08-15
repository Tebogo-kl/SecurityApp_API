using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Secu.DataTransferObjects;
using Secu.Services.Interfaces;

namespace Secu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create(CreateCompanyDto dto)
        {
            var ownerId = Guid.Parse(User.Identity?.Name ?? throw new UnauthorizedAccessException());
            var result = await _companyService.CreateAsync(dto, ownerId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetById(Guid id)
        {
            var result = await _companyService.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var result = await _companyService.GetAllAsync();
            return Ok(result);
        }
    }
}
