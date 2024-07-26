using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public interface IStoreRepository
    {
        Task<Store> CreateAsync(Store store);
        Task<Store> GetByIdAsync(long companyId, long id);
        Task UpdateAsync(Store store);
        Task DeleteAsync(Store store);
        Task<IEnumerable<Store>> GetAllAsync(long companyId);
    }
}