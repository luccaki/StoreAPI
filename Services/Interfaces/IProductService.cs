using StoreAPI.Dtos;

namespace StoreAPI.Services
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(long companyId, long storeId, CreateUpdateProductDto productDto);
        Task<ProductDto> GetByIdAsync(long companyId, long storeId, long id);
        Task UpdateAsync(long companyId, long storeId, long id, CreateUpdateProductDto updateProductDto);
        Task DeleteAsync(long companyId, long storeId, long id);
        Task<IEnumerable<ProductDto>> GetAllAsync(long companyId, long storeId);
        Task<string> GetAllProductsAsJsonAsync(long companyId, long storeId);
    }
}