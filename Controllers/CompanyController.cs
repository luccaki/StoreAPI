using Microsoft.AspNetCore.Mvc;
using StoreAPI.Dtos;
using StoreAPI.Services;

namespace StoreAPI.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CreateUpdateCompanyDto companyDto)
        {
            var company = await _companyService.CreateAsync(companyDto);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(long id)
        {
            var company = await _companyService.GetByIdAsync(id);
            return Ok(company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(long id, CreateUpdateCompanyDto updateCompanyDto)
        {
            await _companyService.UpdateAsync(id, updateCompanyDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(long id)
        {
            await _companyService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyService.GetAllAsync();
            return Ok(companies);
        }
    }
}