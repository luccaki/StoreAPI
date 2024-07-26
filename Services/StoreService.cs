using StoreAPI.Dtos;
using StoreAPI.Models;
using StoreAPI.Repositories;

namespace StoreAPI.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<StoreDto> CreateAsync(long companyId, CreateUpdateStoreDto storeDto)
        {
            var store = new Store()
            {
                Name = storeDto.Name,
                CompanyId = companyId
            };
            var res = await _storeRepository.CreateAsync(store);

            return new StoreDto(res.Id, res.Name, res.CompanyId, null);
        }

        public async Task<StoreDto> GetByIdAsync(long companyId, long id)
        {
            var res = await _storeRepository.GetByIdAsync(companyId, id);
            return new StoreDto(res.Id, res.Name, res.CompanyId, res.Products.Select(p => new ProductDto(p.Id, p.Name, p.Value, p.StoreId)));
        }

        public async Task UpdateAsync(long companyId, long id, CreateUpdateStoreDto updateStoreDto)
        {
            var store = await _storeRepository.GetByIdAsync(companyId, id);

            store.Name = updateStoreDto.Name;

            await _storeRepository.UpdateAsync(store);
        }

        public async Task DeleteAsync(long companyId, long id)
        {
            var store = await _storeRepository.GetByIdAsync(companyId, id);
            await _storeRepository.DeleteAsync(store);
        }

        public async Task<IEnumerable<StoreDto>> GetAllAsync(long companyId)
        {
            var res = await _storeRepository.GetAllAsync(companyId);
            return res.Select(s => new StoreDto(s.Id, s.Name, s.CompanyId, null));
        }
    }
}