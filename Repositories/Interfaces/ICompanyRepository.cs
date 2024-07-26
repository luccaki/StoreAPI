using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> CreateAsync(Company company);
        Task<Company> GetByIdAsync(long id);
        Task UpdateAsync(Company company);
        Task DeleteAsync(Company company);
        Task<IEnumerable<Company>> GetAllAsync();
    }
}