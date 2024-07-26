using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product> GetByIdAsync(long storeId, long id);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync(long storeId);
        Task<string> GetAllProductsAsJsonAsync(long companyId, long storeId);
    }
}