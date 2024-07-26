using StoreAPI.Dtos;
using StoreAPI.Models;

namespace StoreAPI.Services
{
    public interface ICompanyService
    {
        Task<CompanyDto> CreateAsync(CreateUpdateCompanyDto companyDto);
        Task<CompanyDto> GetByIdAsync(long id);
        Task UpdateAsync(long id, CreateUpdateCompanyDto updateCompanyDto);
        Task DeleteAsync(long id);
        Task<IEnumerable<CompanyDto>> GetAllAsync();
    }
}