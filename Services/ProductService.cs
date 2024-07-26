using StoreAPI.Dtos;
using StoreAPI.Models;
using StoreAPI.Repositories;

namespace StoreAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStoreService _storeService;

        public ProductService(IProductRepository productRepository, IStoreService storeService)
        {
            _productRepository = productRepository;
            _storeService = storeService;
        }

        public async Task<ProductDto> CreateAsync(long companyId, long storeId, CreateUpdateProductDto productDto)
        {
            var store = await _storeService.GetByIdAsync(companyId, storeId);
            var product = new Product()
            {
                StoreId = store.Id,
                Name = productDto.Name,
                Value = productDto.Value
            };

            var res = await _productRepository.CreateAsync(product);

            return new ProductDto(res.Id, res.Name, res.Value, res.StoreId);
        }

        public async Task<ProductDto> GetByIdAsync(long companyId, long storeId, long id)
        {
            var store = await _storeService.GetByIdAsync(companyId, storeId);
            var res = await _productRepository.GetByIdAsync(store.Id, id);
            return new ProductDto(res.Id, res.Name, res.Value, res.StoreId);
        }

        public async Task UpdateAsync(long companyId, long storeId, long id, CreateUpdateProductDto updateProductDto)
        {
            var store = await _storeService.GetByIdAsync(companyId, storeId);
            var product = await _productRepository.GetByIdAsync(store.Id, id);

            product.Name = updateProductDto.Name;
            product.Value = updateProductDto.Value;

            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(long companyId, long storeId, long id)
        {
            var store = await _storeService.GetByIdAsync(companyId, storeId);
            var product = await _productRepository.GetByIdAsync(store.Id, id);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync(long companyId, long storeId)
        {
            var res = await _productRepository.GetAllAsync(storeId);
            return res.Select(res => new ProductDto(res.Id, res.Name, res.Value, res.StoreId));
        }
        public async Task<string> GetAllProductsAsJsonAsync(long companyId, long storeId)
        {
            return await _productRepository.GetAllProductsAsJsonAsync(companyId, storeId);
        }
    }
}