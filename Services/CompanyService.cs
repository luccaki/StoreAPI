using StoreAPI.Dtos;
using StoreAPI.Models;
using StoreAPI.Repositories;

namespace StoreAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyDto> CreateAsync(CreateUpdateCompanyDto companyDto)
        {
            var res = await _companyRepository.CreateAsync(new Company { Name = companyDto.Name, Owner = companyDto.Owner });
            return new CompanyDto(res.Id, res.Name, res.Owner, null);
        }

        public async Task<CompanyDto> GetByIdAsync(long id)
        {
            var res = await _companyRepository.GetByIdAsync(id);
            return new CompanyDto(res.Id, res.Name, res.Owner, res.Stores.Select(s => new StoreDto(s.Id, s.Name, s.CompanyId, null)));
        }

        public async Task UpdateAsync(long id, CreateUpdateCompanyDto updateCompanyDto)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            company.Name = updateCompanyDto.Name;
            company.Owner = updateCompanyDto.Owner;

            await _companyRepository.UpdateAsync(company);
        }

        public async Task DeleteAsync(long id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            await _companyRepository.DeleteAsync(company);
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            var res = await _companyRepository.GetAllAsync();
            return res.Select(c => new CompanyDto(c.Id, c.Name, c.Owner, null));
        }
    }
}