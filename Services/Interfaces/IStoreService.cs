using StoreAPI.Dtos;
using StoreAPI.Models;

namespace StoreAPI.Services
{
    public interface IStoreService
    {
        Task<StoreDto> CreateAsync(long companyId, CreateUpdateStoreDto storeDto);
        Task<StoreDto> GetByIdAsync(long companyId, long id);
        Task UpdateAsync(long companyId, long id, CreateUpdateStoreDto updateStoreDto);
        Task DeleteAsync(long companyId, long id);
        Task<IEnumerable<StoreDto>> GetAllAsync(long companyId);
    }
}